(function ($) {
    app.modals.ChuyenDuyetHoSoModal = function () {
        var _quyTrinhDuAnAssignedsService = abp.services.app.quyTrinhDuAnAssigneds;

        var _modalManager;
        var _$quyTrinhDuAnAssignedInformationForm = null;

        $('#keToanTiepNhanId').select2({
            placeholder: 'Chưa chọn kế toán phụ trách',
            allowClear: true
        }).trigger("change");
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$quyTrinhDuAnAssignedInformationForm = _modalManager.getModal().find('form[name=PhieuXuLyHoSoForm]');
            _$quyTrinhDuAnAssignedInformationForm.validate();
        };

        this.save = function () {
            if (!_$quyTrinhDuAnAssignedInformationForm.valid()) {
                return;
            }
           
            var chuyenDuyetHoSo = _$quyTrinhDuAnAssignedInformationForm.serializeFormToObject();

            console.log(chuyenDuyetHoSo);
            _modalManager.setBusy(true);
            _quyTrinhDuAnAssignedsService.xuLyHoSo(
                chuyenDuyetHoSo
            ).done(function (result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.setResult(result);
                _modalManager.close();
                // abp.event.trigger('app.createOrEditQuyTrinhDuAnAssignedModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);