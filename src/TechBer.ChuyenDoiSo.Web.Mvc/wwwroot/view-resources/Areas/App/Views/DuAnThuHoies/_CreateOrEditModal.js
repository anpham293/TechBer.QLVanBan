(function ($) {
    app.modals.CreateOrEditDuAnThuHoiModal = function () {

        var _duAnThuHoiesService = abp.services.app.duAnThuHoies;

        var _modalManager;
        var _$duAnThuHoiInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$duAnThuHoiInformationForm = _modalManager.getModal().find('form[name=DuAnThuHoiInformationsForm]');
            _$duAnThuHoiInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$duAnThuHoiInformationForm.valid()) {
                return;
            }

            var duAnThuHoi = _$duAnThuHoiInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _duAnThuHoiesService.createOrEdit(
				duAnThuHoi
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDuAnThuHoiModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);