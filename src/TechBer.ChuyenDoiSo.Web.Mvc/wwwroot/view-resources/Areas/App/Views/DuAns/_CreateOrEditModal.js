(function ($) {
    app.modals.CreateOrEditDuAnModal = function () {

        var _duAnsService = abp.services.app.duAns;

        var _modalManager;
        var _$duAnInformationForm = null;

        var _DuAnloaiDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/LoaiDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_DuAnLoaiDuAnLookupTableModal.js',
            modalClass: 'LoaiDuAnLookupTableModal'
        });

        var _DuAnChuongLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/ChuongLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_DuAnChuongLookupTableModal.js',
            modalClass: 'ChuongLookupTableModal',
            modalSize: "modal-xl"
        });

        var _DuAnLoaiKhoanLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/LoaiKhoanLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_DuAnLoaiKhoanLookupTableModal.js',
            modalClass: 'LoaiKhoanLookupTableModal',
            modalSize: "modal-xl"
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$duAnInformationForm = _modalManager.getModal().find('form[name=DuAnInformationsForm]');
            _$duAnInformationForm.validate();
        };

        $('#OpenLoaiDuAnLookupTableButton').click(function () {

            var duAn = _$duAnInformationForm.serializeFormToObject();

            _DuAnloaiDuAnLookupTableModal.open({id: duAn.loaiDuAnId, displayName: duAn.loaiDuAnName}, function (data) {
                _$duAnInformationForm.find('input[name=loaiDuAnName]').val(data.displayName);
                _$duAnInformationForm.find('input[name=loaiDuAnId]').val(data.id);
            });
        });

        $('#ClearLoaiDuAnNameButton').click(function () {
            _$duAnInformationForm.find('input[name=loaiDuAnName]').val('');
            _$duAnInformationForm.find('input[name=loaiDuAnId]').val('');
        });

        $('#OpenChuongLookupTableButton').click(function () {

            var duAn = _$duAnInformationForm.serializeFormToObject();

            _DuAnChuongLookupTableModal.open({id: duAn.chuongId, displayName: duAn.chuongName}, function (data) {
                _$duAnInformationForm.find('input[name=chuongName]').val(data.maSo + ' - '+ data.ten);
                _$duAnInformationForm.find('input[name=chuongId]').val(data.id);
            });
        });

        $('#ClearChuongNameButton').click(function () {
            _$duAnInformationForm.find('input[name=chuongName]').val('');
            _$duAnInformationForm.find('input[name=chuongId]').val('');
        });

        $('#OpenLoaiKhoanLookupTableButton').click(function () {

            var duAn = _$duAnInformationForm.serializeFormToObject();

            _DuAnLoaiKhoanLookupTableModal.open({id: duAn.loaiKhoanId, displayName: duAn.loaiKhoanName}, function (data) {
                _$duAnInformationForm.find('input[name=loaiKhoanName]').val(data.maSo + ' - '+ data.ten);
                _$duAnInformationForm.find('input[name=loaiKhoanId]').val(data.id);
            });
        });

        $('#ClearLoaiKhoanNameButton').click(function () {
            _$duAnInformationForm.find('input[name=loaiKhoanName]').val('');
            _$duAnInformationForm.find('input[name=loaiKhoanId]').val('');
        });

        $('#DuAn_TongMucDauTu').inputmask();
        $('#DuAn_DuToan').inputmask();

        $('#DuAn_TongMucDauTu').change(function() {
            var xuLy='';
            xuLy = DocSoThanhChu($('#DuAn_TongMucDauTu').inputmask('unmaskedvalue'));
            $('#DuAn_TongMucDauTuBangChu').val(xuLy);
        });
        $('#DuAn_DuToan').change(function() {
            var xuLy='';
            xuLy = DocSoThanhChu($('#DuAn_DuToan').inputmask('unmaskedvalue'));
            $('#DuAn_DuToanBangChu').val(xuLy);
        });

        this.save = function () {
            if (!_$duAnInformationForm.valid()) {
                return;
            }
            if ($('#DuAn_LoaiDuAnId').prop('required') && $('#DuAn_LoaiDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('LoaiDuAn')));
                return;
            }

            var duAn = _$duAnInformationForm.serializeFormToObject();
            duAn.tongMucDauTu = $('#DuAn_TongMucDauTu').inputmask('unmaskedvalue');
            duAn.duToan = $('#DuAn_DuToan').inputmask('unmaskedvalue');

            _modalManager.setBusy(true);
            _duAnsService.createOrEdit(
                duAn
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDuAnModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);