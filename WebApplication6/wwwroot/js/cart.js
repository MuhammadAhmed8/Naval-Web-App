let quantityChanger = document.getElementById("quantityChanger");
quantityChanger.addEventListener("click", changeProductQuantity);

function changeProductQuantity() {
    console.log("hello")
    $.ajax({
        type: 'get',
        utl: '/Cart/SetQuantity/5',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
       
        success: function (result) {
            console.log(result)
        },
        error: function (err) {
            console.log(err)
        }

    })
}