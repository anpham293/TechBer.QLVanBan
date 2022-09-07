(function ($) {
    app.modals.CreateOrEditVanBanDuAnModal = function () {

        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;

        var _modalManager;
        var _$vanBanDuAnInformationForm = null;
        var uploadedFileToken = null;
        var fileName = "";
        var contentType = "";
        var _VanBanDuAnduAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/DuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/VanBanDuAns/_VanBanDuAnDuAnLookupTableModal.js',
            modalClass: 'DuAnLookupTableModal'
        });
        var _VanBanDuAnquyTrinhDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/QuyTrinhDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/VanBanDuAns/_VanBanDuAnQuyTrinhDuAnLookupTableModal.js',
            modalClass: 'QuyTrinhDuAnLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$vanBanDuAnInformationForm = _modalManager.getModal().find('form[name=VanBanDuAnInformationsForm]');
            _$vanBanDuAnInformationForm.validate();
        };
        $('#VanBanDuAnInformationsForm input[name=fileMau]').change(function (e) {
            if (e.target.files[0]) {
                fileName = e.target.files[0].name;
            } else {
                fileName = "";
            }
            $('#VanBanDuAnInformationsForm').submit();
        });

        $('#VanBanDuAnInformationsForm').ajaxForm({
            beforeSubmit: function (formData, jqForm, options) {
                abp.ui.setBusy('.modal-content');
                var $fileInput = $('#VanBanDuAnInformationsForm input[name=fileMau]');
                console.log($fileInput);
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
        
        $('#OpenDuAnLookupTableButton').click(function () {

            var vanBanDuAn = _$vanBanDuAnInformationForm.serializeFormToObject();

            _VanBanDuAnduAnLookupTableModal.open({
                id: vanBanDuAn.duAnId,
                displayName: vanBanDuAn.duAnName
            }, function (data) {
                _$vanBanDuAnInformationForm.find('input[name=duAnName]').val(data.displayName);
                _$vanBanDuAnInformationForm.find('input[name=duAnId]').val(data.id);
            });
        });

        $('#ClearDuAnNameButton').click(function () {
            _$vanBanDuAnInformationForm.find('input[name=duAnName]').val('');
            _$vanBanDuAnInformationForm.find('input[name=duAnId]').val('');
        });

        $('#OpenQuyTrinhDuAnLookupTableButton').click(function () {

            var vanBanDuAn = _$vanBanDuAnInformationForm.serializeFormToObject();

            _VanBanDuAnquyTrinhDuAnLookupTableModal.open({
                id: vanBanDuAn.quyTrinhDuAnId,
                displayName: vanBanDuAn.quyTrinhDuAnName
            }, function (data) {
                _$vanBanDuAnInformationForm.find('input[name=quyTrinhDuAnName]').val(data.displayName);
                _$vanBanDuAnInformationForm.find('input[name=quyTrinhDuAnId]').val(data.id);
            });
        });

        $('#ClearQuyTrinhDuAnNameButton').click(function () {
            _$vanBanDuAnInformationForm.find('input[name=quyTrinhDuAnName]').val('');
            _$vanBanDuAnInformationForm.find('input[name=quyTrinhDuAnId]').val('');
        });


        this.save = function () {
            if (!_$vanBanDuAnInformationForm.valid()) {
                return;
            }
            if ($('#VanBanDuAn_DuAnId').prop('required') && $('#VanBanDuAn_DuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DuAn')));
                return;
            }
            if ($('#VanBanDuAn_QuyTrinhDuAnId').prop('required') && $('#VanBanDuAn_QuyTrinhDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('QuyTrinhDuAn')));
                return;
            }

            var vanBanDuAn = _$vanBanDuAnInformationForm.serializeFormToObject();
            vanBanDuAn.uploadedFileToken = uploadedFileToken;
            vanBanDuAn.fileName = fileName;
            vanBanDuAn.contentType = contentType;
            
            console.log(uploadedFileToken);
            console.log("vanbanduan:");
            console.log(vanBanDuAn);
            _modalManager.setBusy(true);
            _vanBanDuAnsService.createOrEdit(
                vanBanDuAn
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditVanBanDuAnModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);