(function ($) {
    app.modals.CreateOrEditQuyetDinhModal = function () {

        var _quyetDinhsService = abp.services.app.quyetDinhs;

        var _modalManager;
        var _$quyetDinhInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$quyetDinhInformationForm = _modalManager.getModal().find('form[name=QuyetDinhInformationsForm]');
            _$quyetDinhInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$quyetDinhInformationForm.valid()) {
                return;
            }

            var quyetDinh = _$quyetDinhInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _quyetDinhsService.createOrEdit(
				quyetDinh
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditQuyetDinhModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);