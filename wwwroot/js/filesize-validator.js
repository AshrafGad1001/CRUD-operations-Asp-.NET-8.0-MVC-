$.validator.addMethod('filesize', function (value, element, param) {
    var isValid = (this.optional(element)) || (element.files[0].size <= param);

    return isValid;
});