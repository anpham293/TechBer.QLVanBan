(function ($) {
    app.modals.CreateOrEditLoaiKhoanModal = function () {

        var _loaiKhoansService = abp.services.app.loaiKhoans;

        var _modalManager;
        var _$loaiKhoanInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$loaiKhoanInformationForm = _modalManager.getModal().find('form[name=LoaiKhoanInformationsForm]');
            _$loaiKhoanInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$loaiKhoanInformationForm.valid()) {
                return;
            }

            var loaiKhoan = _$loaiKhoanInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _loaiKhoansService.createOrEdit(
				loaiKhoan
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLoaiKhoanModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);