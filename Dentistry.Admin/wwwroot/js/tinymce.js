tinymce.init({
    selector: '#contentEditor',
    plugins: 'image',
    toolbar: 'image',
    images_upload_url: '/upload-image',
    automatic_uploads: true,
    file_picker_types: 'image',
    file_picker_callback: function (callback, value, meta) {
        var input = document.createElement('input');
        input.setAttribute('type', 'file');
        input.setAttribute('accept', 'image/*');

        debugger;
        input.onchange = function () {
            var file = this.files[0];
            var reader = new FileReader();
            reader.onload = function () {
                callback(reader.result, { alt: file.name });
            };
            reader.readAsDataURL(file);
        };

        input.click();
    }
});