(function ($) {
    app.modals.CreateOrEditDuAnModal = function () {

        var _duAnsService = abp.services.app.duAns;

        var _modalManager;
        var _$duAnInformationForm = null;

		

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

		  

        this.save = function () {
            if (!_$duAnInformationForm.valid()) {
                return;
            }

            var duAn = _$duAnInformationForm.serializeFormToObject();
			
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