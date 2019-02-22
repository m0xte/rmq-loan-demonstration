using Microsoft.AspNetCore.Mvc;

[Controller]
public class HomeController : Controller
{   
    [HttpGet]
    public string Index()
    {
        return "\n\n<h1>Hello, world</h1>\n\n\n";
    } 
}
