var returnValue = "000000";
var currentHue = -1234234;

for(var i = 0; i < HUE_STEPS; i++)
{
    var hue = Math.floor(i / HUE_STEPS * 360);
    var picker = document.createElement("div");
    picker.style.backgroundColor = "hsl(" + hue + ",100%,50%)";
    picker.className = "huePicker";
    picker.setAttribute("data-hue", hue);
    //picker.onclick = function(){selectHue(this.getAttribute("data-hue"))};
    picker.onmousedown = function(){selectHue(this.getAttribute("data-hue"))};
    hueContainer.appendChild(picker);
}
var bPicker = document.createElement("div");
bPicker.className = "huePicker";
bPicker.style.backgroundColor = "#000000";
bPicker.setAttribute("data-hue", hue);
bPicker.onclick = function(){showGrayscale()};
hueContainer.appendChild(bPicker);

function selectHue(hue)
{
    if(currentHue == hue) return;
    currentHue = hue;
    var grid = document.getElementById("grid");
    while(grid.childElementCount > 0)
        grid.removeChild(grid.firstChild);

    for(var i_l = 1; i_l <= L_STEPS; i_l++)
    {
        var light = Math.floor(i_l / L_STEPS * 100);
        for(var i_s = 0; i_s <= SAT_STEPS; i_s++)
        {
            var sat = Math.floor(i_s / SAT_STEPS * 100);
            var cp = document.createElement("div");
            cp.className = "cp";
            cp.style.backgroundColor = "#" + hsvToHex(hue,sat,light);
            cp.setAttribute("data-rgb", hsvToHex(hue,sat,light));
            cp.onclick = function(){selectColor(this.getAttribute("data-rgb"))}
            grid.appendChild(cp);
        }
        var br = document.createElement("br");
        grid.appendChild(br);
    }
}

function selectColor(rgb)
{
    returnValue = rgb;
    window.location.href = "/ok?color=" + returnValue;
}

function showGrayscale()
{
    if(currentHue == -1) return;
    currentHue = -1;
    var grid = document.getElementById("grid");
    while(grid.childElementCount > 0)
        grid.removeChild(grid.firstChild);

    var sat = 0;
    for(var i_s = 0; i_s <= SAT_STEPS; i_s++)
    {
        var light = Math.floor(i_s / SAT_STEPS * 100);
        var cp = document.createElement("div");
        cp.className = "cp";
        cp.style.backgroundColor = "#" + hsvToHex(hue,sat,light);
        cp.setAttribute("data-rgb", hsvToHex(hue,sat,light));
        cp.onclick = function(){selectColor(this.getAttribute("data-rgb"))}
        grid.appendChild(cp);
    }
}

selectHue(0);