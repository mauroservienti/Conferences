using System.Collections.Generic;
public class WhoIAm
{
	string title = "RavenDB: the document database";
	string name = "Mauro Servienti";
	string mail = "mauro@topics.it";
	string twitter = "@mauroservienti";
	string[] blogs = new string[]
	{
		"//milestone.topics.it",
		"//blogs.ugidotnet.org/topics"
	};
	Dictionary<string, string> facts = new Dictionary<string, string>()
	{
		{"Role", "CTO @ Managed Designs"},
		{"MVP", "Visual C#"},
		{"Trainer", "RavenDB, NServiceBus"},
		{"GitHub", "//github.com/mauroservienti"},
	};
}