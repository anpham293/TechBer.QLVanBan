(function ($) {
    app.modals.EditChiTietDanhGiaModal = function () {

        var _doiTuongChuyenDoiSosService = abp.services.app.doiTuongChuyenDoiSos;

        var _modalManager;
        var _$chiTietDanhGiaForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chiTietDanhGiaForm = _modalManager.getModal().find('form[name=ChiTietDanhGiaForm]');
            _$chiTietDanhGiaForm.validate();
        };

		
        this.save = function () {
            if (!_$chiTietDanhGiaForm.valid()) {
                return;
            }

            var chiTietDanhGia = _$chiTietDanhGiaForm.serializeFormToObject();

			_modalManager.setBusy(true);
			_doiTuongChuyenDoiSosService.editChiTietDanhGia(
                chiTietDanhGia
			).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                _modalManager.setResult();
			}).always(function () {
                _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);