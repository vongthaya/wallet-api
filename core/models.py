from django.conf import settings
from django.db import models


class Wallet(models.Model):
    owner = models.OneToOneField(settings.AUTH_USER_MODEL, on_delete=models.PROTECT)
    balance = models.DecimalField(max_digits=18, decimal_places=0, default=0)
