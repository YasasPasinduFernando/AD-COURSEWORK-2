document.addEventListener('DOMContentLoaded', function () {
  document.querySelectorAll('.um-alert.alert-success').forEach(function (el) {
    window.setTimeout(function () {
      if (typeof bootstrap !== 'undefined' && bootstrap.Alert) {
        var instance = bootstrap.Alert.getOrCreateInstance(el);
        instance.close();
      }
    }, 6000);
  });
});
