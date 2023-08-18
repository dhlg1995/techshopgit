var a = $(".rating-avg span").text();
var grstar5 = $("#5start").text();
var grstar4 = $("#4start").text();
var grstar3 = $("#3start").text();
var grstar2 = $("#2start").text();
var grstar1 = $("#1start").text();


console.log(grstar1)
console.log(grstar2)
console.log(grstar3)
console.log(grstar4)
console.log(grstar5)


loadReviews(model);
startFill(a);



$("#5startbar").css({ "width": progressStar(grstar5) })
$("#4startbar").css({ "width": progressStar(grstar4) })
$("#3startbar").css({ "width": progressStar(grstar3) })
$("#2startbar").css({ "width": progressStar(grstar2) })
$("#1startbar").css({ "width": progressStar(grstar1) })
 function  progressStar(_start) {
    var gstart = Number(_start.trim())
    var total = Number($("#total").val());
     var grbarpercent = (gstart / total) * 100;    
    return  grbarpercent
}

var model = new Object();
model.PageSize = 5;
model.PageIndex = 1;
var countLoadAll = 0;
var rvModel = new Object();
function loadReviews(model, isLoadMore = false) {
    $.ajax({
        type: 'post',
        url: '/product/ReviewListPartial?_bool=false',
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (!isLoadMore) {
                $("#reviews").html(data);
            }
            else {
                $("#reviews").append(data);
            }
        }
    });
}function loadAllReviews() {
    $.ajax({
        type: 'get',
        url: '/product/ReviewListPartial?_bool=true',        
        contentType: "application/json; charset=utf-8",
        success: function (response) {           
                $("#reviews").html(response);
        }
    });
}
function submitReview(model) {
   
    $.ajax({
        type: 'post',
        url: '/product/InsertReviews',
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
           window.location.href="/product/detail"
        }
    });
}

function fillStarReview() {

}

function startFill(_rate) {
    var rate = _rate.substring(0, 1);
    switch (rate) {
        case '5':
            $("#rating-stars-sum i").removeClass("fa-star-o").addClass("fa-star")
            $("#main-rating i").removeClass("fa-star-o").addClass("fa-star")
            break;
        case '4':
            $('#start5').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start5').removeClass("fa-star").addClass("fa-star-o")

            break;
        case '3':
            $('#start5').removeClass("fa-star").addClass("fa-star-o")
            $('#start4').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start5').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start4').removeClass("fa-star").addClass("fa-star-o")
            break;
        case '2':
            $('#start5').removeClass("fa-star").addClass("fa-star-o")
            $('#start4').removeClass("fa-star").addClass("fa-star-o")
            $('#start3').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start5').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start4').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start3').removeClass("fa-star").addClass("fa-star-o")
            break;
        case '1':
            $('#start5').removeClass("fa-star").addClass("fa-star-o")
            $('#start4').removeClass("fa-star").addClass("fa-star-o")
            $('#start3').removeClass("fa-star").addClass("fa-star-o")
            $('#start2').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start5').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start4').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start3').removeClass("fa-star").addClass("fa-star-o")
            $('#main-start2').removeClass("fa-star").addClass("fa-star-o")
            break;
        default:
            $('#start5').removeClass("fa-star").addClass("fa-star-o")
            $('#start4').removeClass("fa-star").addClass("fa-star-o")
            $('#start3').removeClass("fa-star").addClass("fa-star-o")
            $('#start2').removeClass("fa-star").addClass("fa-star-o")
            $('#start1').removeClass("fa-star").addClass("fa-star-o")

    }

}



function loadMoreReview() {
   
    model.PageIndex = model.PageIndex + 1;
    loadReviews(model,true);
}
function loadAllRv() {
    switch (countLoadAll) {
        case 1:
            model.PageIndex = 1;
            loadReviews(model);
            countLoadAll = 0;
            break;
        case 0:
            loadAllReviews();
            countLoadAll += 1;
            break;
        default:
            break;
    }  
}
function insertReview() {   
    rvModel.ProductId = $("#rv-producid").val();
    rvModel.ReviewerName = $("#rv-name").val();
    rvModel.Email = $("#rv-email").val();
    rvModel.Content = $("#rv-txt").val();
    if ($("#star1").is(":checked") == true) rvModel.Rating = "1";
    if ($("#star2").is(":checked") == true) rvModel.Rating = "2";
    if ($("#star3").is(":checked") == true) rvModel.Rating = "3";
    if ($("#star4").is(":checked") == true) rvModel.Rating = "4";
    if ($("#star5").is(":checked") == true) rvModel.Rating = "5";
    submitReview(rvModel);
}
$("body").on("click", "#btn-rv-loadmore", loadMoreReview);
$("body").on("click", "#btn-rv-loadall", loadAllRv);
$("body").on("click","#btn-submit", insertReview)

