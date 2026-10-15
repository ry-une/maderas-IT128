using System.Reflection;
using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;

var failures = 0;
foreach (var authenticated in new[] { false, true })
{
    var originalInput = Console.In;
    var originalOutput = Console.Out;
    try
    {
        var store = new FakeStore(authenticated);
        using var input = new StringReader("synthetic-user\nsynthetic-password\nTest title\nTest body\n");
        using var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);
        var program = Assembly.Load("BlogTestUI").GetType("BlogTestUI.Program")!;
        program.GetMethod("AddPost", BindingFlags.NonPublic | BindingFlags.Static)!
            .Invoke(null, new object[] { new SqlData(store) });
        if (store.AuthenticationCalls != 1) throw new Exception("Expected one authentication attempt.");
        if (!authenticated)
        {
            if (store.Writes.Count != 0) throw new Exception("Rejected login wrote a post.");
            if (!output.ToString().Contains("Invalid credentials.")) throw new Exception("Missing rejection message.");
            if (output.ToString().Contains("Title:")) throw new Exception("Rejected login prompted for post content.");
            if (input.ReadLine() != "Test title") throw new Exception("Rejected login consumed post content.");
        }
        else
        {
            if (store.Writes.Count != 1) throw new Exception("Valid login must write exactly one post.");
            var post = store.Writes.Single();
            if (post.UserId != 42 || post.Title != "Test title" || post.Body != "Test body")
                throw new Exception("Post content or authenticated author changed.");
            if (post.DateCreated == default) throw new Exception("Post date was not assigned.");
        }
        originalOutput.WriteLine($"PASS: {(authenticated ? "accepted" : "rejected")} login add-post flow");
    }
    catch (Exception error)
    {
        failures++;
        originalOutput.WriteLine($"FAIL: {(authenticated ? "accepted" : "rejected")} login: {error.GetBaseException().Message}");
    }
    finally
    {
        Console.SetIn(originalInput);
        Console.SetOut(originalOutput);
    }
}
return failures == 0 ? 0 : 1;

sealed class FakeStore : ISqlDataAccess
{
    private readonly bool authenticated;
    public FakeStore(bool authenticated) => this.authenticated = authenticated;
    public int AuthenticationCalls { get; private set; }
    public List<PostModel> Writes { get; } = new();
    public List<T> LoadData<T, U>(string sql, U parameters, string connection, bool storedProcedure)
    {
        if (sql != "dbo.spUsers_Authenticate" || typeof(T) != typeof(UserModel))
            throw new Exception("Unexpected read.");
        AuthenticationCalls++;
        return authenticated ? new List<T> { (T)(object)new UserModel { Id = 42 } } : new List<T>();
    }
    public void SaveData<T>(string sql, T parameters, string connection, bool storedProcedure)
    {
        if (sql != "spPosts_Insert") throw new Exception("Unexpected write.");
        var value = (object)parameters!;
        object Read(string name) => value.GetType().GetProperty(name)!.GetValue(value)!;
        Writes.Add(new PostModel
        {
            UserId = (int)Read("UserId"), Title = (string)Read("Title"),
            Body = (string)Read("Body"), DateCreated = (DateTime)Read("DateCreated")
        });
    }
}
