// for main tag's height for all pages
document.getElementsByTagName('main')[0].style.minHeight = (window.screen.height - 400)+ "px";

// hide footer tag when 404 page is active
if (document.getElementById("notFound")) {
    document.getElementById("footer").style.display = "none";
    document.getElementById("header").style.display = "none";
    document.getElementById("Humberger").style.display = "none";
}

function addToCart(addToCartBtn, productId, text=0) {
    $.ajax({
        contentType: "application/json",
        headers: {
            "isApiResponse": true
        },
        url: `/Customer/Cart/AddToCart?productId=${productId}`,
        type: "POST",
        success: function (data) {
            // If text === 1, then "Added!" text will be added
            // If not, button will be hidden
            if (text === 1) {
                addToCartBtn.innerText = "ADDED !";
                addToCartBtn.disabled = true;
                addToCartBtn.style.cursor = "no-drop";
            } else {
                addToCartBtn.style.display = "none";
            }
            Toastify({
                text: data.success,
                duration: 3000
            }).showToast();
            return true;
        },
        failure: function (response) {
            alert(response.responseText);
            return false;
        },
        error: function (error) {
            if (error.status === 401) {
                return window.location.href = "https://" + window.location.host + "/Identity/Account/Login?ReturnUrl=/Customer/Shops";
            }
            console.log(error);
            return false;
        }
    });
}

function addOrRemoveToWishlist(wishlistBtn, productId) {
    $.ajax({
        contentType: "application/json",
        headers: {
            "isApiResponse": true
        },
        method: "GET",
        url: `/Customer/Wishlist/AddOrDeleteWishlist?productId=${productId}`,
        success: function (data) {
            wishlistBtn.style.display = "none";
            Toastify({
                text: data.success,
                duration: 2000
            }).showToast();
        },
        error: function (error) {
            console.log("Error");
            if (error.status === 401) {
                return window.location.href = "https://" + window.location.host + "/Identity/Account/Login?ReturnUrl=" + window.location.pathname;
            }
            console.log(error);
        }
    });
}

function handleChange() {
    var selectedValue = document.getElementById("sortProduct").value;
    console.log("Selected value: " + selectedValue);
    if (selectedValue === "Default") document.getElementById("Default").click();
    if (selectedValue === "High_To_Low") document.getElementById("High_To_Low").click();
    if (selectedValue === "Low_To_High") document.getElementById("Low_To_High").click();
    if (selectedValue === "New") document.getElementById("New").click();
}