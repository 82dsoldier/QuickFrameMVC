(function() {
  var hideOverlay, showOverlay;

  showOverlay = function() {
    return $('#overlay').addClass('overlay');
  };

  hideOverlay = function() {
    return $('#overlay').removeClass('overlay').addClass('overlay-off');
  };

  $(document).ready(function() {
    return $('input[type=text]').first().focus();
  });

}).call(this);
