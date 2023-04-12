(function ($) {
    app.modals.CreateOrEditPhongKhoModal = function () {

        var _phongKhosService = abp.services.app.phongKhos;

        var _modalManager;
        var _$phongKhoInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$phongKhoInformationForm = _modalManager.getModal().find('form[name=PhongKhoInformationsForm]');
            _$phongKhoInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$phongKhoInformationForm.valid()) {
                return;
            }

            var phongKho = _$phongKhoInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _phongKhosService.createOrEdit(
				phongKho
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPhongKhoModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);