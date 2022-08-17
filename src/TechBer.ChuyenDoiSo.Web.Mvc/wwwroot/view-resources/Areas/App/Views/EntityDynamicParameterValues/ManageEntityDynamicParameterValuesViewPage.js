(function () {
    $(function () {
        var _manageDynamicParameterValueBase = new ManageDynamicParameterValueBase();

        function initializePage() {
            _manageDynamicParameterValueBase.initialize({
                entityFullName: $("#EntityFullName").val(),
                entityId: $("#EntityId").val(),
                bodyElement: $('#EntityDynamicParameterValuesTable').find("tbody"),
                onDeleteValues: function() {
                    setTimeout(
                        function() {
                            window.location.reload();
                        }, 500)
                }
            });
        }
        
        function saveParameters() {
            _manageDynamicParameterValueBase.save();
        }

        $("#saveParameters").click(saveParameters);

        initializePage();
    });
})();
