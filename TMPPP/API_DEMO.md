# TMPPP API Demo

Acest API ruleaza peste aceeasi logica de business existenta (invoice + payment + process).
Include si UI web pentru demo, astfel incat poti arata totul din browser.
UI-ul include acum sectiuni dedicate pentru `Adapter Pattern Demo` si `Composite Pattern Demo`.

## Pornire API

```bash
cd TMPPP
PAYMENT_STORAGE=memory dotnet run -- --api
```

Alternativ, pentru SQLite (fisier `payments.db`):

```bash
cd TMPPP
dotnet run -- --api
```

UI Demo: `http://localhost:5000/` (sau portul afisat in consola)
Swagger UI: `http://localhost:5000/swagger`

## Endpoint-uri principale

- `GET /api/health`
- `GET /api/payment-methods`
- `GET /api/patterns/adapter-demo`
- `GET /api/patterns/composite-demo`
- `POST /api/patterns/composite-build`
- `POST /api/invoices`
- `GET /api/invoices`
- `POST /api/payments`
- `POST /api/payments/{paymentId}/process`
- `POST /api/demo/run`

## Demo din browser (recomandat la prezentare)

1. Deschide `http://localhost:5000/`
2. Apasa `Run Adapter Demo` pentru a prezenta integrarea unitara PayPal / Stripe / Google Pay
3. Foloseste `Composite Pattern Demo` pentru a adauga plati pe grupuri si a construi un batch nou direct din UI
4. Optional: `Run Composite Demo` pentru exemplul presetat
5. Apasa `Create Invoice` (se completeaza automat `Invoice ID` la payment)
6. Apasa `Create Payment` (se completeaza automat `Payment ID` la process)
7. Apasa `Process Payment`
8. Optional: `Run Full Demo` pentru flux complet intr-un click

## Demo rapid (cu curl)

1. Creeaza invoice:

```bash
curl -s -X POST http://localhost:5000/api/invoices \
  -H 'Content-Type: application/json' \
  -d '{"amount":150,"currency":"RON"}'
```

2. Creeaza payment (inlocuieste `INVOICE_ID`):

```bash
curl -s -X POST http://localhost:5000/api/payments \
  -H 'Content-Type: application/json' \
  -d '{"invoiceId":"INVOICE_ID","amount":150,"currency":"RON"}'
```

3. Proceseaza payment (inlocuieste `PAYMENT_ID`):

```bash
curl -s -X POST http://localhost:5000/api/payments/PAYMENT_ID/process \
  -H 'Content-Type: application/json' \
  -d '{"method":"cash"}'
```

4. Ruleaza tot fluxul dintr-un foc:

```bash
curl -s -X POST http://localhost:5000/api/demo/run
```
