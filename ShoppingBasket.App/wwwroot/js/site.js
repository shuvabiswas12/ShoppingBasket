// for main tag's height for all pages
document.getElementsByTagName('main')[0].style.minHeight = (window.screen.height - 400)+ "px";


// hide footer tag when 404 page is active
if (document.getElementById("notFound")) {
    document.getElementById("footer").style.display = "none";
    document.getElementById("header").style.display = "none";
    document.getElementById("Humberger").style.display = "none";
}