Today I made some minor changes to this blog to allow me to write posts with frequent ease. About 2 years ago, before I made the switch to Wordpress, I was using Google's blogging platform for my [last blog](http://dotnetcaffeine.blogspot.co.nz/). I tried to make use of a variety of tools that promised to make blogging easier for me, like [Windows Live Writer](https://www.microsoft.com/en-nz/download/details.aspx?id=8621). While it delivered on its basic features, I still found the markup it generated to be inconsistent, gory and hard to follow. This coupled with issues like hosting, uploading & linking to external resources, pictures etc. put me off writing anything I wanted to share.

I compiled a list of things I have done off late which make the whole process a lot easier, convenient and cheaper.

## Edit your posts using Markdown
I would like to blog more, mainly so that I have someplace I can log all the stuff I do on a daily basis. Off late I have been using [Markdown](http://daringfireball.net/projects/markdown/) a lot. Its a syntax which makes text-to-html conversion real easy for anyone that produces content for the web. I love how it allows me to focus on my content and not have to worry about how my content is styled. [Latex](http://latex-project.org/intro.html) anyone?

I noticed that WordPress officially supports writing posts in Markdown, and the feature is available on all its pricing tier's (yea im on the free tier, bite me!). Its not enabled by default but you can easily enable the feature by following these [set of instructions](https://en.support.wordpress.com/markdown/#writing-with-markdown). Once this is done, you can write all your posts directly using its online editor using markdown syntax. Now I can enjoy just focusing on what I have to say without having to worry about my post looking right. You can find a list of the WordPress's markdown support listed in the [Markdown quick reference](https://en.support.wordpress.com/markdown-quick-reference/) 

![screen1](http://piransworld.blob.core.windows.net/blog-images/my-2-cents-on-making-blogging-easier/screen1.PNG)

If you are a developer, WordPress offers some great features for [posting your source code](https://en.support.wordpress.com/code/posting-source-code/) in your blog. You automatically get syntax highlighting support for a number of languages aside other features like source code titles, linking, line highlighting, padding line numbers etc all of which make it a great platform for a developer's blog.

[code language="csharp"]
public void PrintName(string name){
	Console.WriteLine("Hello " + name );
}
[/code]

## Create Blog posts using.. anything?
Now that I have markdown support enabled, I find that I dont even need to write all my posts online using the browser. For example at the time of writing this post, I am simply using [Sublime](http://www.sublimetext.com/). It's free, has support for markdown syntax highlighting. But in reality you can use just about any text editor of your choice to write your blog posts. Theoretically I can write when I am online, offline, using my phone, in a terminal (using *Vi*). Sure you may want to keep referring to the markdown syntax reference guides if you are new to markdown and ocassionally preview your post before publishing it, but once you get profficient with its syntax, you should find your blogging productivity skyrocket. Heres a screen-shot of me writing this blog post:
![screen2](http://piransworld.blob.core.windows.net/blog-images/my-2-cents-on-making-blogging-easier/screen2.PNG)

## Linking to external resources, pictures
You may want to insert some pictures, serve additional resources via your blog. However the [Basic plan](https://store.wordpress.com/plans/) on WordPress does not offer a lot of space. You have to pay a lot more $$ if you want to get more hosting space for your blog. You can easily get around this by using a cloud storage service. I love using Microsoft's [Azure Blob Storage](http://azure.microsoft.com/en-us/services/storage/) service which in my opinion is very competitively priced. At the time of writing, it costs something along the lines of $0.0588 (NZD) per GB for a geographically redundant storage plan. So I simply created a new Geographically redundant Azure blob store and using tools like the [Cloudberry Azure Blob Explorer](http://www.cloudberrylab.com/free-microsoft-azure-explorer.aspx) I can now easily upload all my external content & images to the cloud and link to my resources directly through my posts.

For information on how to use Azure blob storage, refer to the [documentation](http://azure.microsoft.com/en-us/documentation/services/storage/).

## Backing up your posts, resources etc
So the final piece of this puzzle is being able to store & access all your posts and resources for future reference. Given that I am now creating and editing all my posts using markdown, I can literally use any modern day version control system to store, version and track all my posts and content. I just use Git & Github to backup all my posts. Using a simple directory structure to organize all my content by each post, I can now easily access and backup all my posts and external content easily. In future, should I want to move to a different blog engine, I can easily do so with a full history of all my posts securely available online. Head on over to my [Github](https://github.com/pirahawk/piransworld) repo to check out my setup.


Happy blogging ;)