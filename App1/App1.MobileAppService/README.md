## MobileAppService

### Default User Secrets
```
{
  "ConnectionStrings:DefaultConnection": "Data Source=mydb.db;",
  "Jwt": {
    "Key": "SuperTopSecretKeyThatYouDoNotGiveOutEver!",
    "Issuer": "http://localhost:5000/",
    "Audience": "http://localhost:5000/",
    "ExpireTime": 86400
  }
}
```