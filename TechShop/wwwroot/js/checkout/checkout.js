$('body').on('click', '#place-order-btn', handlePlaceOrder)
function handlePlaceOrder() {
    console.log('inside');
    var form = $('#frm-customer-info');
    form.submit();
}