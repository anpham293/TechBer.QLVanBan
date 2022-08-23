(function ($) {
    app.modals.CreateOrEditQuyTrinhDuAnModal = function () {

        var _quyTrinhDuAnsService = abp.services.app.quyTrinhDuAns;

        var _modalManager;
        var _$quyTrinhDuAnInformationForm = null;

        var _QuyTrinhDuAnloaiDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAns/LoaiDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAns/_QuyTrinhDuAnLoaiDuAnLookupTableModal.js',
            modalClass: 'LoaiDuAnLookupTableModal'
        });
        var _QuyTrinhDuAnquyTrinhDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAns/QuyTrinhDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAns/_QuyTrinhDuAnQuyTrinhDuAnLookupTableModal.js',
            modalClass: 'QuyTrinhDuAnLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$quyTrinhDuAnInformationForm = _modalManager.getModal().find('form[name=QuyTrinhDuAnInformationsForm]');
            _$quyTrinhDuAnInformationForm.validate();
        };

        $('#OpenLoaiDuAnLookupTableButton').click(function () {

            var quyTrinhDuAn = _$quyTrinhDuAnInformationForm.serializeFormToObject();

            _QuyTrinhDuAnloaiDuAnLookupTableModal.open({
                id: quyTrinhDuAn.loaiDuAnId,
                displayName: quyTrinhDuAn.loaiDuAnName
            }, function (data) {
                _$quyTrinhDuAnInformationForm.find('input[name=loaiDuAnName]').val(data.displayName);
                _$quyTrinhDuAnInformationForm.find('input[name=loaiDuAnId]').val(data.id);
            });
        });

        $('#ClearLoaiDuAnNameButton').click(function () {
            _$quyTrinhDuAnInformationForm.find('input[name=loaiDuAnName]').val('');
            _$quyTrinhDuAnInformationForm.find('input[name=loaiDuAnId]').val('');
        });

        $('#OpenQuyTrinhDuAnLookupTableButton').click(function () {

            var quyTrinhDuAn = _$quyTrinhDuAnInformationForm.serializeFormToObject();

            _QuyTrinhDuAnquyTrinhDuAnLookupTableModal.open({
                id: quyTrinhDuAn.parentId,
                displayName: quyTrinhDuAn.quyTrinhDuAnName
            }, function (data) {
                _$quyTrinhDuAnInformationForm.find('input[name=quyTrinhDuAnName]').val(data.displayName);
                _$quyTrinhDuAnInformationForm.find('input[name=parentId]').val(data.id);
            });
        });

        $('#ClearQuyTrinhDuAnNameButton').click(function () {
            _$quyTrinhDuAnInformationForm.find('input[name=quyTrinhDuAnName]').val('');
            _$quyTrinhDuAnInformationForm.find('input[name=parentId]').val('');
        });


        this.save = function () {
            if (!_$quyTrinhDuAnInformationForm.valid()) {
                return;
            }
            if ($('#QuyTrinhDuAn_LoaiDuAnId').prop('required') && $('#QuyTrinhDuAn_LoaiDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('LoaiDuAn')));
                return;
            }
            if ($('#QuyTrinhDuAn_ParentId').prop('required') && $('#QuyTrinhDuAn_ParentId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('QuyTrinhDuAn')));
                return;
            }

            var quyTrinhDuAn = _$quyTrinhDuAnInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _quyTrinhDuAnsService.createOrEdit(
                quyTrinhDuAn
            ).done(function (result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.setResult(result);
                _modalManager.close();
                abp.event.trigger('app.createOrEditQuyTrinhDuAnModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);