class AppSettings {
  static get webApiUrl() {
    switch (process.env.NODE_ENV) {
      case 'development':
        return 'https://localhost:5001';
      default:
        return '';
    }
  }
}
export default AppSettings;
