$(document).ready(function(){

//Initializing arrays with city names
    var SYLHET = [];
    var DHAKA = [];
    var CHITTAGONG = [];
    var BARISHAL = [];
    var KHULNA = [];
    var MYMENSINGH = [];
    var RAJSHAHI = [];
    var RANGPUR = [];

var FRANCE = [
    {display: "Paris", value: "Paris" }, 
    {display: "Avignon", value: "Avignon" }, 
    {display: "Strasbourg", value: "Strasbourg" },
    {display: "Nice", value: "Nice" }];

//Function executes on change of first select option field 
$("#country").change(function () {
    $.ajax({
        url: 'http://localhost:52240/api/values/',
        method: 'GET',
        success: function (officeList) {
            console.log(officeList);

            officeList.forEach(function (office) {
                var str = office.officeId;
                
                if (str.search("Dha") !== -1) {
                        console.log(str);
                        DHAKA.push(str);
                }
                else if (str.search("Chi") !== -1) {
                    //alert(str);
                    console.log(str);
                    CHITTAGONG.push(str);
                }
                else if (str.search("Bar") !== -1) {
                    //alert(str);
                    console.log(str);
                    BARISHAL.push(str);
                }
                else if (str.search("Khu") !== -1) {
                    //alert(str);
                    console.log(str);
                    KHULNA.push(str);
                }
                else if (str.search("Mym") !== -1) {
                    //alert(str);
                    console.log(str);
                    MYMENSINGH.push(str);
                }
                else if (str.search("Raj") !== -1) {
                    //alert(str);
                    console.log(str);
                    RAJSHAHI.push(str);
                }
                else if (str.search("Ran") !== -1) {
                    //alert(str);
                    console.log(str);
                    RANGPUR.push(str);
                }
                else if (str.search("Syl") !== -1) {
                    //alert(str);
                    console.log(str);
                    SYLHET.push(str);
                }
            });
        }
    });
    console.log(DHAKA);
var select = $("#country option:selected").val();
    console.log("sele : " + select);
    //alert("sele : " + select);

switch(select){
    case "SYLHET":
        city(SYLHET);
        break;

    case "DHAKA":
        //alert(DHAKA);
        city(DHAKA);
        break;

    case "CHITTAGONG":
        city(CHITTAGONG);
        break;

    case "BARISHAL":
        city(BARISHAL);
        break;

    case "KHULNA":
        city(KHULNA);
        break;

    case "MYMENSINGH":
        city(MYMENSINGH);
        break;

    case "RAJSHAHI":
        city(RAJSHAHI);
        break;

    case "RANGPUR":
        city(RANGPUR);
        break;

default:
	$("#city").empty();
	$("#city").append("<option>--Select--</option>");
break;
}
});

//Function To List out Cities in Second Select tags
function city(arr){
	$("#city").empty();//To reset cities
	$("#city").append("<option>--Select--</option>");
	//$(arr).each(function(i){//to list cities
	//	$("#city").append("<option value=\""+arr[i]+"\">"+arr[i]+"</option>")
    //   });
    console.log("arr " + arr);
    for (var i = 0; i < arr.length; i++) {
        $("#city").append("<option value=\"" + arr[i] + "\">" + arr[i] + "</option>")  //a b c
    }
}

});