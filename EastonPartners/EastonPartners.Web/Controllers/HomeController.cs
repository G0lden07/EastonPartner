using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Interfaces;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using EastonPartners.Web.Models;
using System.Linq;
using Markdig;

namespace EastonPartners.Web.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController<HomeController>
{
    private readonly IOpenAIService _ai;

    public HomeController (IOpenAIService ai)
    {
        _ai = ai;
    }

	public IActionResult Index()
    {
        return View();
    }
	public IActionResult Help()
	{
		return View();
	}
	public IActionResult Assistant()
	{
		return View();
    }

    public async Task<IActionResult> Chat(string message)
    {
        ChatViewModel vm = new ChatViewModel { Response = string.Empty };

        var completionResult = await _ai.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                //ChatMessage.FromSystem("You are a helpful assistant."),
                //ChatMessage.FromUser("Who won the world series in 2020?"),
                //ChatMessage.FromAssistant("The Los Angeles Dodgers won the World Series in 2020."),
                ChatMessage.FromUser(message)
            },
            Model = OpenAI.ObjectModels.Models.Gpt_4o,
        });
        if (completionResult.Successful)
        {
            //Console.WriteLine(completionResult.Choices.First().Message.Content);
            vm.Response = Markdown.ToHtml( completionResult.Choices.First().Message.Content );
        }

        return View(vm);
    }
}