(function ($) {
    app.modals.CreateOrEditBaoCaoVanBanDuAnModal = function () {

        var _baoCaoVanBanDuAnsService = abp.services.app.baoCaoVanBanDuAns;

        var _modalManager;
        var _$baoCaoVanBanDuAnInformationForm = null;

		        var _BaoCaoVanBanDuAnvanBanDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BaoCaoVanBanDuAns/VanBanDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BaoCaoVanBanDuAns/_BaoCaoVanBanDuAnVanBanDuAnLookupTableModal.js',
            modalClass: 'VanBanDuAnLookupTableModal'
        });        var _BaoCaoVanBanDuAnuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BaoCaoVanBanDuAns/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BaoCaoVanBanDuAns/_BaoCaoVanBanDuAnUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$baoCaoVanBanDuAnInformationForm = _modalManager.getModal().find('form[name=BaoCaoVanBanDuAnInformationsForm]');
            _$baoCaoVanBanDuAnInformationForm.validate();
        };

		          $('#OpenVanBanDuAnLookupTableButton').click(function () {

            var baoCaoVanBanDuAn = _$baoCaoVanBanDuAnInformationForm.serializeFormToObject();

            _BaoCaoVanBanDuAnvanBanDuAnLookupTableModal.open({ id: baoCaoVanBanDuAn.vanBanDuAnId, displayName: baoCaoVanBanDuAn.vanBanDuAnName }, function (data) {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnName]').val(data.displayName); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearVanBanDuAnNameButton').click(function () {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnName]').val(''); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var baoCaoVanBanDuAn = _$baoCaoVanBanDuAnInformationForm.serializeFormToObject();

            _BaoCaoVanBanDuAnuserLookupTableModal.open({ id: baoCaoVanBanDuAn.userId, displayName: baoCaoVanBanDuAn.userName }, function (data) {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userName]').val(data.displayName); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userName]').val(''); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userId]').val(''); 
        });


        //Begin ImportFile

        var uploadedFileToken = null;
        var fileName = "";
        var contentType = "";

        $('#BaoCaoVanBanDuAnInformationsForm input[name=fileMau]').change(function (e) {
            if (e.target.files[0]) {
                fileName = e.target.files[0].name;
            } else {
                fileName = "";
            }
            $('#BaoCaoVanBanDuAnInformationsForm').submit();
        });
        $('#BaoCaoVanBanDuAnInformationsForm').ajaxForm({
            beforeSubmit: function (formData, jqForm, options) {
                abp.ui.setBusy('.modal-content');
                var $fileInput = $('#BaoCaoVanBanDuAnInformationsForm input[name=fileMau]');
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
        $('#BaoCaoVanBanDuAn_FileBaoCao').fileupload({
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
            if (!_$baoCaoVanBanDuAnInformationForm.valid()) {
                return;
            }
            if ($('#BaoCaoVanBanDuAn_VanBanDuAnId').prop('required') && $('#BaoCaoVanBanDuAn_VanBanDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('VanBanDuAn')));
                return;
            }
            if ($('#BaoCaoVanBanDuAn_UserId').prop('required') && $('#BaoCaoVanBanDuAn_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var baoCaoVanBanDuAn = _$baoCaoVanBanDuAnInformationForm.serializeFormToObject();
            baoCaoVanBanDuAn.uploadedFileToken = uploadedFileToken;
            baoCaoVanBanDuAn.fileName = fileName;
            baoCaoVanBanDuAn.contentType = contentType;
			
			 _modalManager.setBusy(true);
			 _baoCaoVanBanDuAnsService.createOrEdit(
				baoCaoVanBanDuAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBaoCaoVanBanDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);