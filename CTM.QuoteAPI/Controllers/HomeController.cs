using Microsoft.AspNetCore.Mvc;

[Controller]
public class HomeController : Controller
{   
    [HttpGet]
    public string Index()
    {
        return "<h1>Hello, world</h1>";
    } 
}
