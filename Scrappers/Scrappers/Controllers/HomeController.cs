using System.Diagnostics;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Scrappers.Models;

namespace Scrappers.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    public IActionResult Scrapper()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Scrapper(string url)
    {
        var web = new HtmlWeb();
        var document = web.Load(url);
        List<dynamic> liste = new();
        string title = "";
        string price = "";

        if (document.DocumentNode.QuerySelector("div.pdp-base > h1") != null)
        {
            title = document.DocumentNode.QuerySelector("div.pdp-base > h1").InnerText.Trim(); //. class ismine denk geliyor
            price = document.DocumentNode.QuerySelector("div[class^='pdp-prc2']>span").InnerText.Trim();
            ViewBag.name = title;
            ViewBag.price = price;
            liste.Add(new { title, price });
        }
        else
        {
            var productItems = document.DocumentNode.QuerySelectorAll("div.prd");

            foreach (var products in productItems)
            {
                title = products.Attributes["data-product-name"].Value;
                price = products.Attributes["data-product-price"].Value;
                liste.Add(new { title, price });

            }
        }
        return View(liste);
    }

    public IActionResult TrustPilot()
    {
        return View();
    }
    [HttpPost]
    public IActionResult TrustPilot(string url)
    {
        var web = new HtmlWeb();
        var document = web.Load(url);
        List<dynamic> liste = new List<dynamic>();
        string name = "";
        string comment = "";
        string title = "";
        string commentEncode = "";
        string nameEncode = "";
        string titleEncode = "";
        string star = "";
        string starNumber = "";
        string time = "";
        int counter = 0;
        int sumCounter=0;

        //document.querySelector("div.styles_reviewContent__0Q2Tg>p"); comment
        //document.querySelector("div.styles_reviewContent__0Q2Tg>h2"); title
        //document.querySelector("div.styles_consumerDetailsWrapper__p2wdr>a>div")

        var allComments = document.DocumentNode.QuerySelectorAll("section.styles_reviewsContainer__3_GQw > div>article");

        foreach (var comments in allComments)
        {
            // "//*[@id=\"my_control_id\"
            //kontrol yapılacak 
            name = comments.QuerySelector("div.styles_consumerDetailsWrapper__p2wdr>a>div").InnerText;

            star = comments.QuerySelector("div.styles_reviewHeader__iU9Px>div>img").Attributes["alt"].Value;
            if (star.Contains("Rated 1"))
                starNumber = "1 Star";
            else if (star.Contains("Rated 2"))
                starNumber = "2 Stars";
            else if (star.Contains("Rated 3"))
                starNumber = "3 Stars";
            else if (star.Contains("Rated 4"))
                starNumber = "4 Stars";
            else if (star.Contains("Rated 5"))
                starNumber = "5 Stars";

            HtmlNode htmlNodetime = comments.QuerySelector("section.styles_reviewContentwrapper__zH_9M>div>div>time");
            if (htmlNodetime != null)
                time = comments.QuerySelector("section.styles_reviewContentwrapper__zH_9M>div>div>time").Attributes["datetime"].Value;
            else
                time = null;

            HtmlNode htmlNodeH2 = comments.QuerySelector("div.styles_reviewContent__0Q2Tg>h2");
            if (htmlNodeH2 != null)
            {
                title = comments.QuerySelector("div.styles_reviewContent__0Q2Tg>h2").InnerText;
                titleEncode = System.Web.HttpUtility.HtmlDecode(title);
            }

            else
                titleEncode = "empty";

            HtmlNode htmlNodeP = comments.QuerySelector("div.styles_reviewContent__0Q2Tg>p");
            if (htmlNodeP != null)
            {
                comment = comments.QuerySelector("div.styles_reviewContent__0Q2Tg>p").InnerText;
                commentEncode = System.Web.HttpUtility.HtmlDecode(comment);

            }

            else
                commentEncode = null;





            liste.Add(new { name, starNumber, commentEncode, titleEncode,time });

        }


        return View(liste);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

