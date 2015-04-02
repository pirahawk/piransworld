While working on some sites, I recently picked up a couple of approaches that help with vertically and horizontally centering elements of arbitrary dimensions for certain situations. I have added a couple of them here and will update this list as I come across some more in the future.

# Scenario 1: Horizontally centering block elements within a container
Assume we have a `.parent` element which in turn wraps 2 `.child` div elements of arbitrary width and height:

[code language="html" title=scenario1.htm"]
<!DOCTYPE html>
<html>
<head>
	<style>
		.parent {
            background-color: red;
        }

        .blue {
            background-color: blue;
            width: 50px;
            height: 60px;
        }

        .green {
            background-color: green;
            width: 60px;
            height: 50px;
        }
	</style>

</head>
<body>
	<div class="parent">
        <div class="child blue">blue</div>
        <div class="child green">green</div>
    </div>
</body>
</html>
[/code]

![screen1](http://piransworld.blob.core.windows.net/blog-images/horizontally-and-vertically-centering-elements-with-css/screen1.PNG)

When viewed in a browser, this should display as shown above. Our aim is to have all the `.child` elements line up horizontally in the center of the `.parent` div element. This can easily be achieved via css, by instructing the browser to equally distribute the horizontal `margin` of all `.child` elements like so

[code language="css"]
.parent > .child {
	margin: 0 auto;   
}
[/code]

When viewed in a browser, we should see something similar to:
![screen2](http://piransworld.blob.core.windows.net/blog-images/horizontally-and-vertically-centering-elements-with-css/screen2.PNG)

**Note** that this technique only works when the width of the block level element is fixed. This is because by default, block level elements always expand to fill all available horizontal space of its parent element. This can be illustrated by the addition of a third `.child` element without a fixed `width`
![screen3](http://piransworld.blob.core.windows.net/blog-images/horizontally-and-vertically-centering-elements-with-css/screen3.PNG)

# Scenario 2: Vertically and horizontally centering inline elements within a container of fixed dimensions

Assume we have the following situation where the `.parent` container has a fixed width and height of 400px and in turn wraps three inline(img) `.child` elements of arbitrary dimensions.

[code language="html" title=scenario2.htm"]
<!DOCTYPE html>
<html>
<head>
    <style>
        .parent {
            background-color: red;
            height: 400px;
            width: 400px;
        }
    </style>

</head>
<body>
    <div class="parent">
        <img class="child" src="http://lorempixel.com/50/50/transport/" alt="img" />
        <img class="child" src="http://lorempixel.com/50/60/transport/" alt="img" />
        <img class="child" src="http://lorempixel.com/50/90/transport/" alt="img" />
    </div>
</body>
</html>
[/code]

When viewed in a browser, we should see something similar to:
![screen4](http://piransworld.blob.core.windows.net/blog-images/horizontally-and-vertically-centering-elements-with-css/screen4.PNG)

## Step 1: Vertically align all the Children within the container

To vertically align all the `.child` elements within the center of the `.parent`, you will need to adjust its css `display` property to change the parent element's display behaviour:

[code language="css"]
.parent {
background-color: red;
height: 400px;
width: 400px;

display: table-cell;
vertical-align:middle;
text-align: center;
}
[/code]

The `table-cell` display behavior will force the alignment of all child content within the div to respect the same semantics seen when using the `<td>` element within an html table (for a detailed explanation, please refer to this awesome [guide on understanding the Table element](https://css-tricks.com/complete-guide-table-element/) ). 

We can then easily employ the `vertical-align:middle` property which instructs the browser to vertically center the all child elements in the middle of the containing `div`. By default, `img` elements are rendered as inline elements, hence we need to use the `text-align: center` property in turn to horizontally center them. The result should look something like:
![screen5](http://piransworld.blob.core.windows.net/blog-images/horizontally-and-vertically-centering-elements-with-css/screen5.PNG)

## Step 2: Vertically align all the Children along the center axis

Notice how all the `.child` images are aligned along their bottom edges. This is a by-product of the inline display mode semantics. We can take this one step further and vertically align the images themselves along their middle baseline. *Note* that this can only be achieved if you are confident of giving at least one of the child elements a fixed dimension. In our case we can easily pick a max height of ~90px (i.e. the height of the largest image).

[code language="css"]
.child {
display:inline-block;
vertical-align:middle;
max-height:90px;
}
[/code]

![screen6](http://piransworld.blob.core.windows.net/blog-images/horizontally-and-vertically-centering-elements-with-css/screen6.PNG)

Grab all completed examples from my [Github repo](https://github.com/pirahawk/piransworld/tree/master/horizontally-and-vertically-centering-elements-with-css)


