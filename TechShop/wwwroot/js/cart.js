var a = $('.fa fa-shopping-cart')

function displayCart() {
    console.log("inside");
    $.ajax({
        type: 'post',
        url: '/Cart/ViewCartPartial',
        success: function (data) {
            
            $('#cart-inner-html').html(data);
        }
    })
   // $('#cart-inner-html').html(data);
}


    function AddtoCart() {
        var z = $(this).attr("product-id");    
        console.log(z);
        $.ajax({
            type: 'post',
            url: '/Cart/AddToCart/?productId=' + z,
            success: function(data){
                if (data == 200) {
                    alert("add cart success!");
                }
            }
        })
        displayCart();
}
$('body').on('click', '.add-to-cart-btn', AddtoCart);
displayCart();