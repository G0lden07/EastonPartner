﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EastonPartners.Domain.Entities.Identity;
using EastonPartners.Web.Areas.Dashboard.Models.HomeViewModels;
using EastonPartners.Web.Controllers;
using RoverCore.BreadCrumbs.Services;
using System.Threading.Tasks;
using OpenAI.Interfaces;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using Markdig;
using EastonPartners.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EastonPartners.Web.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController<HomeController>
{
    private readonly UserManager<ApplicationUser> _userManager;
	private readonly IOpenAIService _ai;

	public HomeController(UserManager<ApplicationUser> userManager, IOpenAIService ai)
	{
		_userManager = userManager;
		_ai = ai;
	}

	public async Task<IActionResult> Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("Home");

        var viewModel = new HomeViewModel
        {
            User = await _userManager.GetUserAsync(User)
        };

        _toast.Success($"Welcome back {viewModel.User.FirstName}!");

        return View(viewModel);
    }

	public async Task<IActionResult> Chat(string message)
	{
		ChatViewModel vm = new ChatViewModel { Response = string.Empty };
		string partnerData;
		string partnerTypeData;

		// Reads all partner entries in json format
		using (StreamReader streamReader = new StreamReader(".\\Seeder\\partners.json"))
		{
			partnerData = streamReader.ReadToEnd();
		}

		// Reads all partner type entries in json format
		using (StreamReader streamReader = new StreamReader(".\\Seeder\\partnertypes.json"))
		{
			partnerTypeData = streamReader.ReadToEnd();
		}

		// Gives the AI a message and the information it needs to generate a response
		var completionResult = await _ai.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
		{
			Messages = new List<ChatMessage>
			{
				// System instructions
				ChatMessage.FromSystem("You are a helpful assistant. This app is called RoverPartners. It's main purpose is to allow Easton Area School District to view and edit information about their business and community partners. On the home page there's a header that has a few buttons. Listing the buttons on the left side of the header from left to right: \"RoverPartners\" just takes you to the homepage, \"Home\" also just takes you to the homepage. \"School District Website\" takes you to the school district website, and \"Help\" takes you to a help page. On the right side of the header, there are mainly two buttons. The first one just switches between dark and light mode. When you're in light mode, the button looks like a moon, when you're in dark mode, it looks like the sun. Next to it is either the login button, which just takes you to the login screen; or it's a button that displays a dropdown that gives you the options: Dashboard, Edit Profile, and Logout. Whether it's the login button or a drop down depends on whether you're logged in or not. On the homepage there's a greeting message and either a login button or Dashboard button depending on whether you're logged in or not. There's also a footer with quick links that take you to home, dashboard or login, and help. The help and login page both have the same header and footer as the homepage. On the help page there's a group of explanations and instructions, and two buttons above the instructions that switch between the help page and assistant page, and the assistant page is where you, the assistant are. It has the same header and footer as the home, help, and login pages. When you log in, you are taken to the Dashboard, which its header only has the RoverPartners button to the left, and the light/dark mode and profile dropdown on the right. The dashboard has no footer either. If you're a normal user, you will see a partner button on the dashboard, and a button that takes you to the help page. If you're an admin, you also have access to three extra buttons/pages, the Partner Type, Users, and Roles pages. They all work in the same way. There's a sidebar on the left of each of these pages, including the dashboard, allowing you to navigate between them easily. The last option on the sidebar is a link to the documentation of the app. Underneath the buttons on the dashboard is another chat prompt that allows the user to communicate with you, just like the one on the assistant page. Each one of these pages display a datatable with all the entries in them. For example the Partners page shows you a datatable with all the partners in it with all of their information. To add an entry, click on the blue create button on the top right of the datatable. Next to each entry in the datatable, there's an actions drop down which gives you three options: Details, which just shows you a page with all the information in that specific entry; Edit, which allows you to edit the information in an entry; and Delete, which permenantly deletes an entry from the database. "),
				ChatMessage.FromSystem("Here's the information from the Help menu in case you need it: RoverPartners is a web app used by Easton Area School District to store and view information about its business and community partners. Only Admins are allowed to edit the information in the database, while users are allowed to view them.\r\n\r\nHow do I make an account?\r\nYou don't. This app is fully administered by Easton Area School District, and only the school district is authorized to create accounts and give them to school staff. If you're a manager at the school district you will be given a username and password to access the app.\r\n\r\nHow do I view the information?\r\nIf you log in, you should be redirected to your home page (Dashboard) where you can click on a button from the options that are avilable to you. If you need to see the partners table, click on the button that says \"partners.\"\r\n\r\nIf you're an Admin you can also edit the information, but only if management tells you that something needs to be changed.\r\n\r\nIf I'm an Admin, how do I edit the information?\r\nTo add a new row to the datatables, click on the blue button on the topright side of the datatable. You should be able to add new information to the table.\r\n\r\nNOTE: You should NEVER change anything in the partners datatable unless you're told to.\r\n\r\nIf you face any technical problems using this app, email the tech department and notify them of the issue."),
				ChatMessage.FromSystem("Here's all the data in the partners datatable in JSON format: \n" + partnerData),
				ChatMessage.FromSystem("Here's all the data in the partner types datatable in JSON format: \n" + partnerTypeData),
				ChatMessage.FromSystem("If the user requests information from the database you can use the information in these JSON files to give them what they want. If you need the partner type of a partner, just look for the partner type in the partner type table with the same partner type id as the one listed for the partner that you're trying to find its type. Make sure you don't mention or provide the user with an entry's ID."),
				ChatMessage.FromUser(message) // User message
			},
			Model = OpenAI.ObjectModels.Models.Gpt_4o, // Specifies which model is being used
		});
		if (completionResult.Successful)
		{
			// Stores the response in a view model object
			vm.Response = Markdown.ToHtml(completionResult.Choices.First().Message.Content);
		}

		return View(vm);
	}
}