var selectedColor;
setC(currentColor);

showOld.style.backgroundColor = getColor(currentColor);
var cc = document.getElementById("cc");
for(var i = 0; i < colors.length; i++)
{
    var cp = document.createElement("div");
    cp.className = "cp";
    cp.onclick = function(){select(this)};
    cp.style.backgroundColor = getColor(colors[i]);
    cp.setAttribute("data-color", JSON.stringify(colors[i]));
    cc.appendChild(cp);
}

function select(cp)
{
    var color = JSON.parse(cp.getAttribute("data-color"));
    color.r = parseInt(color.r);
    color.g = parseInt(color.g);
    color.b = parseInt(color.b);
    setC(JSON.parse(cp.getAttribute("data-color")));
}

function setC(color)
{
    selectedColor = color;
    showNew.style.backgroundColor = getColor(selectedColor);
    var inpColor = document.getElementsByName("color")[0];
    inpColor.value = color.r + "," + color.g + "," + color.b;
}

function showTab(page)
{
    var divs = document.getElementsByName("tabpage");
    for(var i = 0; i < divs.length; i++) divs[i].style.display = "none";
    page.style.display = "block";
}

function getColor(color)
{
    var r = color.r.toString(16);
    var g = color.g.toString(16);
    var b = color.b.toString(16);
    if(color.r < 16) r = '0'.concat(r);
    if(color.g < 16) g = '0' + g;
    if(color.b < 16) b = '0' + b;
    return '#' + r + g + b;
}

function setSliders()
{
    sliderR.value = selectedColor.r;
    sliderG.value = selectedColor.g;
    sliderB.value = selectedColor.b;
}

function sliderChanged(ver)
{
    if(ver == 1)
        selectedColor.r = parseInt(sliderR.value);
    if(ver == 2)
        selectedColor.g = parseInt(sliderG.value);
    if(ver == 3)
        selectedColor.b = parseInt(sliderB.value);
    
    setC(selectedColor);
}