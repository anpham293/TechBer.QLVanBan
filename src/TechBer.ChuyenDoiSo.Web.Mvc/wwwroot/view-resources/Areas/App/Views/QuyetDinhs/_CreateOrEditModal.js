(function ($) {
    app.modals.CreateOrEditQuyetDinhModal = function () {

        var _quyetDinhsService = abp.services.app.quyetDinhs;

        var _modalManager;
        var _$quyetDinhInformationForm = null;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$quyetDinhInformationForm = _modalManager.getModal().find('form[name=QuyetDinhInformationsForm]');
            _$quyetDinhInformationForm.validate();
        };
        
        //Begin ImportFile

        var uploadedFileToken = null;
        var fileName = "";
        var contentType = "";
        
        $('#QuyetDinhInformationsForm input[name=fileMau]').change(function (e) {
            if (e.target.files[0]) {
                fileName = e.target.files[0].name;
            } else {
                fileName = "";
            }
            $('#QuyetDinhInformationsForm').submit();
        });
        $('#QuyetDinhInformationsForm').ajaxForm({
            beforeSubmit: function (formData, jqForm, options) {
                abp.ui.setBusy('.modal-content');
                var $fileInput = $('#QuyetDinhInformationsForm input[name=fileMau]');
                var files = $fileInput.get()[0].files;

                if (!files.length) {
                    uploadedFileToken = null;
                    contentType = "";
                    fileName = "";
                    return false;
                }

                var file = files[0];

                // File size check
                if (file.size > 524288000) { //500MB
                    console.log('file>500')
                    abp.message.warn(app.localize('FileUpload_Warn_SizeLimit', app.constsSoHoa.maxFileBytesUserFriendlyValue));
                    uploadedFileToken = null;
                    contentType = "";
                    fileName = "";
                    return false;
                }

                var mimeType = _.filter(formData, { name: 'fileMau' })[0].value.type;
                $('#fileName').html(file.name);
                formData.push({ name: 'FileType', value: mimeType });
                formData.push({ name: 'FileName', value: file.name });
                formData.push({ name: 'FileToken', value: app.guid() });
                return true;
            },
            success: function (response) {
                if (response.success) {
                    uploadedFileToken = response.result.fileToken;
                    contentType = response.result.fileType;
                } else {
                    abp.message.error(response.error.message);
                }
                abp.ui.clearBusy('.modal-content');

            }
        });
        // End ImportFile

        //Progress File Upload
        $('#QuyetDinh_FileQuyetDinh').fileupload({
            dataType: 'json',
            maxFileSize: 524288000,
            progressall: function (e, data) {
                console.log(e);
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#progress .progress-bar').css(
                    'width',
                    progress + '%'
                );
            }
        });


        this.save = function () {
            if (!_$quyetDinhInformationForm.valid()) {
                return;
            }

            var quyetDinh = _$quyetDinhInformationForm.serializeFormToObject();
            quyetDinh.uploadedFileToken = uploadedFileToken;
            quyetDinh.fileName = fileName;
            quyetDinh.contentType = contentType;
            
			 _modalManager.setBusy(true);
			 _quyetDinhsService.createOrEdit(
				quyetDinh
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditQuyetDinhModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);