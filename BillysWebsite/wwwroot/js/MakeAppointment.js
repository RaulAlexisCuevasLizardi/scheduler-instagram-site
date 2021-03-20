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
});