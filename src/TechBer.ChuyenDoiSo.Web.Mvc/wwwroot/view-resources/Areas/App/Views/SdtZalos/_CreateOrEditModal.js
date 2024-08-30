(function ($) {
    app.modals.CreateOrEditSdtZaloModal = function () {

        var _sdtZalosService = abp.services.app.sdtZalos;

        var _modalManager;
        var _$sdtZaloInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$sdtZaloInformationForm = _modalManager.getModal().find('form[name=SdtZaloInformationsForm]');
            _$sdtZaloInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$sdtZaloInformationForm.valid()) {
                return;
            }

            var sdtZalo = _$sdtZaloInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _sdtZalosService.createOrEdit(
				sdtZalo
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditSdtZaloModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);