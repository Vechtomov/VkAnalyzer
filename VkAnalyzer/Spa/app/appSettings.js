class AppSettings {
  static get webApiUrl() {
    switch (process.env.NODE_ENV) {
      case 'development':
        return 'http://localhost:5555';
      default:
        return '';
    }
  }
}
export default AppSettings;
