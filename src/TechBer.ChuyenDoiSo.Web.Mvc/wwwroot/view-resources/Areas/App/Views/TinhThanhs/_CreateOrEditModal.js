(function ($) {
    app.modals.CreateOrEditTinhThanhModal = function () {

        var _tinhThanhsService = abp.services.app.tinhThanhs;

        var _modalManager;
        var _$tinhThanhInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$tinhThanhInformationForm = _modalManager.getModal().find('form[name=TinhThanhInformationsForm]');
            _$tinhThanhInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$tinhThanhInformationForm.valid()) {
                return;
            }

            var tinhThanh = _$tinhThanhInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _tinhThanhsService.createOrEdit(
				tinhThanh
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditTinhThanhModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);