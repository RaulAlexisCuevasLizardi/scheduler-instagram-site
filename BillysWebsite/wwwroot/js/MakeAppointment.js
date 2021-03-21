function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#referenceImage').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]); // convert to base64 string
        $("#referenceImage").show();
    }
}

$(document).ready(function () {
    $("#imageReferenceUpload").change(function () {
        readURL(this);
    });

    $("#AppointmentType").change(function () {
        var element = $("#AppointmentType option:selected");
        var durationType = element.data("durationtype");
        var duration = element.data("duration");
        var startTime = element.data("starttime");
        var price = element.data("price");
        var priceDecimal = parseFloat(price);
        if (durationType == "HOURS") {
            durationType = " Hour(s)";
        } else {
            durationType = " Minute(s)";
        }
        if (priceDecimal == 0.0000) {
            price = "FREE";
            $("#Price").text("Price: " + price);
        } else {
            $("#Price").text("Price: $" + Math.round(priceDecimal * 100) / 100);
        }
        $("#StartTime").text("Start time: " + startTime);
        $("#Duration").text("Duration: " + duration + durationType);
        $("#AppointmentTypeInformation").show();
    });
});