(function ($) {
    app.modals.CreateOrEditCapQuanLyModal = function () {

        var _capQuanLiesService = abp.services.app.capQuanLies;

        var _modalManager;
        var _$capQuanLyInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$capQuanLyInformationForm = _modalManager.getModal().find('form[name=CapQuanLyInformationsForm]');
            _$capQuanLyInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$capQuanLyInformationForm.valid()) {
                return;
            }

            var capQuanLy = _$capQuanLyInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _capQuanLiesService.createOrEdit(
				capQuanLy
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCapQuanLyModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);