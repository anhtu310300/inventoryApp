# ElectroHub Inventory OS

## Application Flow and Module Guide

**Product type:** Inventory and operations management system for an electronics e-commerce store in India  
**Frontend:** Angular  
**Backend:** ASP.NET Core Web API  
**Database:** MySQL  
**Primary locale:** English (India)  
**Currency:** Indian Rupee (INR)  
**Timezone:** Indian Standard Time (IST)  

---

## 1. Application Purpose

ElectroHub Inventory OS provides one workspace for managing employees, products, stock, procurement, sales, invoices, payments, and business revenue.

The system is designed for an Indian electronics retailer that sells through multiple channels:

- Online store
- Physical retail stores
- Third-party marketplaces
- Direct business-to-business sales

The initial product scope contains three core management areas:

1. Employee Management
2. Inventory Management
3. Revenue Management

The operational layer connects these areas through four supporting modules:

1. Sales Orders
2. Purchase Orders
3. Invoices & Payments
4. Audit Logs

---

## 2. High-Level System Flow

```text
Employee signs in
       │
       ▼
Role and permissions are loaded
       │
       ▼
Dashboard displays relevant KPIs
       │
       ├── Employee Management
       ├── Inventory Management
       ├── Revenue Management
       └── Operations
             ├── Sales Orders
             ├── Purchase Orders
             ├── Invoices & Payments
             └── Audit Logs
```

All business actions follow the same technical pattern:

```text
Angular screen
    │
    │ HTTPS / JSON
    ▼
ASP.NET Core API
    │
    ├── Authentication and authorisation
    ├── Business validation
    ├── Transaction handling
    └── Audit logging
    │
    ▼
MySQL database
```

---

## 3. User Roles and Access

### Administrator

Has access to all modules and system configuration.

Typical responsibilities:

- Create and manage employee accounts
- Assign roles and permissions
- View all warehouses and financial information
- Review security and audit events
- Configure system settings

### Warehouse Manager

Manages products, inventory, purchase orders, and stock receipts.

Typical responsibilities:

- Monitor stock levels
- Create purchase orders
- Receive supplier shipments
- Perform stock adjustments
- Review low-stock and out-of-stock alerts

### Inventory Associate

Handles day-to-day warehouse operations with limited approval rights.

Typical responsibilities:

- Receive and count stock
- Update product locations
- Prepare orders for fulfilment
- Report damaged or missing items

### Sales Team Lead

Manages sales orders and supervises sales employees.

Typical responsibilities:

- Create and update sales orders
- Approve discounts or cancellations
- Monitor order fulfilment
- Review team sales performance

### Sales Executive

Creates and processes customer orders.

Typical responsibilities:

- Create customer orders
- Confirm customer and delivery information
- Select payment methods
- Check product availability

### Accountant

Manages invoices, payments, refunds, and revenue reporting.

Typical responsibilities:

- Generate GST invoices
- Reconcile payments
- Monitor outstanding invoices
- Process refunds
- Export financial reports

---

## 4. Navigation Structure

```text
Dashboard
├── Employees
├── Inventory
├── Revenue
│
├── Operations
│   ├── Sales Orders
│   ├── Purchase Orders
│   ├── Invoices & Payments
│   └── Audit Logs
│
└── System
    └── Settings
```

The sidebar is the primary navigation. Selecting a module replaces the main content area without reloading the entire application.

The top navigation provides:

- Global search
- Theme switcher
- Notifications
- Current user profile
- Account and sign-out actions

---

## 5. Authentication and Application Entry Flow

### Sign-in flow

1. The employee enters an email address and password.
2. Angular sends the credentials to the authentication API.
3. The .NET backend verifies the password hash and account status.
4. The backend returns an access token, refresh token, employee profile, and permissions.
5. Angular stores the session securely and loads the permitted navigation items.
6. The employee is redirected to the Dashboard.
7. A successful sign-in event is written to `audit_log`.

### Access control flow

Every protected action is checked twice:

1. Angular hides or disables actions that the user cannot perform.
2. The .NET API independently validates the user permission before changing data.

Frontend permission checks improve usability, but backend permission checks provide the actual security boundary.

### Account states

- Active
- On Leave
- Suspended
- Deactivated

A suspended or deactivated employee cannot create a new session.

---

## 6. Dashboard Module

### Purpose

The Dashboard provides a quick operational summary and directs the user to areas requiring attention.

### Main information

- Monthly revenue
- Total sales orders
- Total active products
- Low-stock product count
- Revenue and expense trend
- Inventory status distribution
- Recent sales orders
- Recent system activity

### Data sources

| Dashboard widget | Main data source |
|---|---|
| Monthly revenue | `sales`, `invoices`, `payments` |
| Sales order count | `sales` |
| Product count | `products` |
| Low-stock count | `products` and inventory balances |
| Revenue chart | `sales`, `payments`, `expenses` |
| Recent orders | `sales`, `customers` |
| Recent activity | `audit_log` |

### Dashboard flow

1. Angular loads all dashboard summary APIs in parallel.
2. The backend aggregates current-period data.
3. The page displays KPIs, charts, and recent activity.
4. Clicking a widget navigates to the related module with a relevant filter.

Example: clicking **Low Stock** opens Inventory with the status filter set to `LOW_STOCK`.

---

## 7. Employee Management Module

### Purpose

Manages employee identity, department, role, status, and access to the application.

### Main functions

- View employee list
- Search by name, email, or employee ID
- Filter by department and status
- Create an employee
- Edit employee information
- Assign a role
- Suspend or reactivate an account
- Review employee activity

### Employee creation flow

1. An administrator selects **Add Employee**.
2. The administrator enters the employee's personal and work information.
3. A department and role are selected.
4. Angular validates required fields and email format.
5. The API checks that the email and employee ID are unique.
6. The API creates the user and employee profile in one transaction.
7. An account activation or password setup link is sent to the employee.
8. An `EMPLOYEE_CREATED` event is written to `audit_log`.

### Employee update flow

1. An administrator opens an employee record.
2. The administrator changes profile, role, or status information.
3. The backend validates that the acting user has sufficient authority.
4. The employee record is updated.
5. Existing sessions may be revoked if the role or account status changed.
6. The before and after values are stored in the audit event.

### Suggested departments

- Sales
- Warehouse
- Finance
- Administration
- Customer Support
- Procurement

### Primary tables

- `users`
- `audit_log`

For a production schema, consider separating authentication and employee profile data into `users`, `employees`, `roles`, and `user_roles`.

---

## 8. Inventory Management Module

### Purpose

Maintains the product catalogue and the current stock position across warehouses.

### Main functions

- Create and edit products
- Organise products by category
- Track SKU, selling price, and cost price
- Track available stock
- Define reorder levels
- View low-stock and out-of-stock products
- Receive new stock
- Record stock adjustments
- Associate products with suppliers

### Product lifecycle

```text
Draft
  │
  ▼
Active ─────► Low Stock ─────► Out of Stock
  │                │                 │
  │                └──── Restock ────┘
  │
  ▼
Discontinued
```

### Product creation flow

1. The user selects **Add Product**.
2. The user enters the product name, SKU, category, cost price, selling price, and supplier.
3. The backend verifies that the SKU is unique.
4. The backend validates that the selling price and cost price are not negative.
5. The product is created.
6. If opening stock is greater than zero, an opening inventory transaction is created.
7. The action is written to `audit_log`.

### Stock receipt flow

1. A warehouse employee opens an approved purchase order.
2. The employee selects **Receive Stock**.
3. Actual quantities are entered for each purchase item.
4. Damaged or missing quantities are recorded separately.
5. The backend creates the stock receipt in a database transaction.
6. Product inventory is increased.
7. Purchase item received quantities are updated.
8. The purchase order becomes partially received or received.
9. The stock receipt is added to the audit log.

### Stock adjustment flow

Stock adjustments are used for:

- Damaged goods
- Cycle-count corrections
- Lost stock
- Returned goods
- Manual corrections

Every adjustment must include:

- Product
- Warehouse
- Quantity change
- Adjustment reason
- User
- Timestamp
- Optional supporting note

Stock should not be changed directly without creating an inventory transaction.

### Primary tables

- `products`
- `suppliers`
- `purchases`
- `purchase_items`
- `audit_log`

Recommended additional production tables:

- `categories`
- `warehouses`
- `inventory_balances`
- `inventory_transactions`
- `stock_adjustments`
- `product_suppliers`

---

## 9. Sales Orders Module

### Purpose

Manages customer purchases from order creation through payment, fulfilment, delivery, cancellation, or return.

### Main functions

- Create a sales order
- Select customer and delivery address
- Add products and quantities
- Apply discounts
- Calculate GST
- Select sales channel
- Record payment method
- Allocate stock
- Track fulfilment and delivery
- Cancel or return an order

### Sales order lifecycle

```text
Draft
  │
  ▼
Confirmed
  │
  ├──► Cancelled
  │
  ▼
Processing
  │
  ▼
Packed
  │
  ▼
Shipped / In Transit
  │
  ├──► Returned
  │
  ▼
Delivered
  │
  ▼
Completed
```

### Sales order creation flow

1. A sales employee selects **Create Order**.
2. The customer is selected or created.
3. Products and quantities are added.
4. The backend checks available stock.
5. Prices, discounts, taxable value, GST, and order total are calculated by the backend.
6. The user selects the sales channel and payment method.
7. The order is created with a `CONFIRMED` or `PAYMENT_PENDING` status.
8. Stock is reserved for the order.
9. A GST invoice may be generated immediately for prepaid orders.
10. The event is written to `audit_log`.

### Fulfilment flow

1. Warehouse employees see confirmed orders in the picking queue.
2. Items are picked and packed.
3. Reserved stock becomes issued stock.
4. Shipment and tracking information are added.
5. The order status becomes `IN_TRANSIT`.
6. Delivery confirmation changes the status to `DELIVERED` or `COMPLETED`.

### Cancellation flow

1. The user requests a cancellation.
2. The backend checks the current fulfilment state.
3. Reserved stock is released if the order has not shipped.
4. A refund is initiated for a prepaid order.
5. The sales order status becomes `CANCELLED`.
6. The reason and user are recorded in `audit_log`.

### Primary tables

- `customers`
- `sales`
- `sale_items`
- `invoices`
- `payments`
- `products`
- `audit_log`

---

## 10. Purchase Orders Module

### Purpose

Controls product replenishment from supplier selection through approval, shipment, and warehouse receipt.

### Main functions

- Create purchase orders
- Select supplier and destination warehouse
- Add purchase items
- Apply supplier cost and tax information
- Submit orders for approval
- Track expected delivery
- Receive full or partial shipments
- Close or cancel purchase orders

### Purchase order lifecycle

```text
Draft
  │
  ▼
Pending Approval
  │
  ├──► Rejected
  │
  ▼
Approved
  │
  ▼
Sent to Supplier
  │
  ▼
In Transit
  │
  ▼
Partially Received
  │
  ▼
Received / Closed
```

### Purchase order creation flow

1. The warehouse manager reviews low-stock alerts.
2. The manager creates a purchase order.
3. A supplier and destination warehouse are selected.
4. Products, quantities, unit costs, and expected delivery dates are entered.
5. The backend calculates subtotal, tax, and total.
6. The order is saved as draft or submitted for approval.
7. An authorised employee approves the purchase order.
8. The approved purchase order is sent to the supplier.
9. Each state change is written to `audit_log`.

### Primary tables

- `suppliers`
- `purchases`
- `purchase_items`
- `products`
- `audit_log`

---

## 11. Invoices & Payments Module

### Purpose

Manages GST-compliant sales invoices, incoming payments, refunds, and customer receivables.

### Main functions

- Generate a GST invoice
- Calculate CGST/SGST or IGST
- Record payment transactions
- Support UPI, cards, net banking, and cash on delivery
- Track outstanding and overdue invoices
- Download or email invoices
- Process full or partial refunds
- Export the payment ledger

### GST calculation flow

For a sale within the seller's state:

```text
Taxable value
    + CGST
    + SGST
    = Invoice total
```

For an interstate sale:

```text
Taxable value
    + IGST
    = Invoice total
```

The backend must determine the tax type from the seller state and place of supply. The frontend should display the calculation returned by the backend rather than calculate final tax independently.

### Invoice creation flow

1. The user selects a confirmed sales order.
2. Customer billing details and place of supply are loaded.
3. The backend calculates the taxable value and applicable GST.
4. A sequential financial-year invoice number is generated.
5. The invoice is saved and linked to the sales order.
6. A printable invoice document becomes available.
7. The action is recorded in `audit_log`.

Example invoice number:

```text
INV/26-27/00984
```

### Payment flow

1. The customer selects a payment method.
2. The payment provider processes the transaction.
3. The API receives a verified success, failure, or pending result.
4. A payment record is created.
5. The invoice balance is updated.
6. A fully settled invoice becomes `PAID`.
7. A failed payment leaves the invoice as `PAYMENT_PENDING`.
8. Payment events are recorded in `audit_log`.

### Payment statuses

- Pending
- Authorised
- Paid
- Failed
- Partially Refunded
- Refunded

### Primary tables

- `invoices`
- `payments`
- `sales`
- `customers`
- `audit_log`

---

## 12. Revenue Management Module

### Purpose

Provides financial visibility across sales, product costs, operating expenses, tax, and gross profit.

### Main functions

- Display total revenue
- Display gross profit and profit margin
- Calculate average order value
- Compare revenue periods
- Show daily and monthly trends
- Rank top-selling products
- Review recent transactions
- Export management reports

### Core calculations

```text
Net Revenue = Paid Sales - Discounts - Refunds

Cost of Goods Sold = Sum of cost price for sold items

Gross Profit = Net Revenue - Cost of Goods Sold

Gross Margin % = Gross Profit / Net Revenue × 100

Average Order Value = Net Revenue / Completed Orders
```

Taxes collected from customers should be reported separately and should not be treated as business revenue.

### Revenue reporting flow

1. The user selects a month, quarter, or financial year.
2. Angular sends the selected period and optional filters to the API.
3. The backend aggregates paid sales, refunds, product costs, and expenses.
4. Summary KPIs and chart series are returned.
5. Angular updates the cards, charts, product rankings, and transaction table.
6. The user may export the result as Excel, CSV, or PDF.

### Primary tables

- `sales`
- `sale_items`
- `invoices`
- `payments`
- `expenses`
- `products`

---

## 13. Audit Logs Module

### Purpose

Provides a permanent trace of security-sensitive and business-critical actions.

### Events that should be recorded

- Sign-in success or failure
- Employee creation or role change
- Employee suspension or reactivation
- Product creation or price change
- Stock receipt or adjustment
- Sales order creation or cancellation
- Purchase order creation or approval
- Invoice generation
- Payment, refund, or payment failure
- Data export
- System configuration change

### Recommended audit fields

| Field | Description |
|---|---|
| `id` | Unique event identifier |
| `created_at` | Event timestamp in UTC |
| `user_id` | User who performed the action |
| `action` | Standard action code |
| `module` | Source module |
| `entity_type` | Affected entity type |
| `entity_id` | Affected entity ID |
| `old_values` | Previous values as JSON |
| `new_values` | New values as JSON |
| `result` | Success, warning, or failed |
| `ip_address` | Client IP address |
| `user_agent` | Browser or client information |

Audit events should be append-only. Normal application users must not be allowed to edit or delete them.

---

## 14. Notifications

### Notification examples

- Product reached its reorder level
- Product is out of stock
- Purchase order requires approval
- Supplier delivery is overdue
- Sales order payment failed
- Invoice is overdue
- Refund completed
- Suspicious sign-in attempt detected

### Notification flow

1. A business event occurs.
2. The backend evaluates notification rules.
3. A notification record is created for the relevant users or roles.
4. Angular retrieves unread notifications.
5. The notification badge is updated.
6. The user opens or marks the notification as read.

---

## 15. Database Entity Relationships

The initial database tables shown in the prototype can be connected as follows:

```text
users
  ├── creates ───────────────► sales
  ├── creates ───────────────► purchases
  └── generates ─────────────► audit_log

customers
  └── places ────────────────► sales

sales
  ├── contains ──────────────► sale_items
  ├── generates ─────────────► invoices
  └── receives ──────────────► payments

products
  ├── referenced by ─────────► sale_items
  └── referenced by ─────────► purchase_items

suppliers
  └── receives ──────────────► purchases

purchases
  └── contains ──────────────► purchase_items

expenses
  └── contributes to ────────► financial reporting
```

### Suggested key relationships

| Child table | Foreign key | Parent table |
|---|---|---|
| `sales` | `customer_id` | `customers` |
| `sales` | `created_by` | `users` |
| `sale_items` | `sale_id` | `sales` |
| `sale_items` | `product_id` | `products` |
| `purchases` | `supplier_id` | `suppliers` |
| `purchases` | `created_by` | `users` |
| `purchase_items` | `purchase_id` | `purchases` |
| `purchase_items` | `product_id` | `products` |
| `invoices` | `sale_id` | `sales` |
| `payments` | `invoice_id` | `invoices` |
| `payments` | `sale_id` | `sales` |
| `audit_log` | `user_id` | `users` |

---

## 16. Suggested Angular Structure

```text
src/app/
├── core/
│   ├── auth/
│   ├── guards/
│   ├── interceptors/
│   ├── layouts/
│   └── services/
│
├── shared/
│   ├── components/
│   ├── directives/
│   ├── pipes/
│   ├── models/
│   └── utilities/
│
├── features/
│   ├── dashboard/
│   ├── employees/
│   ├── inventory/
│   ├── revenue/
│   ├── sales-orders/
│   ├── purchase-orders/
│   ├── invoices/
│   ├── audit-logs/
│   └── settings/
│
└── app.routes.ts
```

Each feature should normally contain:

```text
feature-name/
├── pages/
├── components/
├── services/
├── models/
├── validators/
└── feature.routes.ts
```

### Suggested Angular routes

```text
/dashboard
/employees
/employees/:id
/inventory
/inventory/products/:id
/revenue
/sales-orders
/sales-orders/:id
/purchase-orders
/purchase-orders/:id
/invoices
/invoices/:id
/audit-logs
/settings
```

---

## 17. Suggested .NET Backend Structure

```text
src/
├── ElectroHub.Api/
│   ├── Controllers/
│   ├── Middleware/
│   └── Program.cs
│
├── ElectroHub.Application/
│   ├── Features/
│   ├── Interfaces/
│   ├── DTOs/
│   └── Validators/
│
├── ElectroHub.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── Events/
│   └── Rules/
│
└── ElectroHub.Infrastructure/
    ├── Persistence/
    ├── Authentication/
    ├── Payments/
    ├── Notifications/
    └── Reporting/
```

### Backend responsibilities

- Controllers receive HTTP requests.
- Application services execute use cases.
- Domain objects enforce business rules.
- Infrastructure handles MySQL, authentication, email, payments, and external services.
- Database transactions protect operations that change several tables.
- Audit logging is applied to critical actions.

---

## 18. Suggested API Groups

### Authentication

```text
POST   /api/auth/login
POST   /api/auth/refresh
POST   /api/auth/logout
GET    /api/auth/me
```

### Employees

```text
GET    /api/employees
GET    /api/employees/{id}
POST   /api/employees
PUT    /api/employees/{id}
PATCH  /api/employees/{id}/status
PATCH  /api/employees/{id}/role
```

### Products and inventory

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
GET    /api/inventory
GET    /api/inventory/low-stock
POST   /api/inventory/receipts
POST   /api/inventory/adjustments
```

### Sales orders

```text
GET    /api/sales-orders
GET    /api/sales-orders/{id}
POST   /api/sales-orders
PATCH  /api/sales-orders/{id}/status
POST   /api/sales-orders/{id}/cancel
POST   /api/sales-orders/{id}/return
```

### Purchase orders

```text
GET    /api/purchase-orders
GET    /api/purchase-orders/{id}
POST   /api/purchase-orders
PUT    /api/purchase-orders/{id}
POST   /api/purchase-orders/{id}/submit
POST   /api/purchase-orders/{id}/approve
POST   /api/purchase-orders/{id}/receive
```

### Invoices and payments

```text
GET    /api/invoices
GET    /api/invoices/{id}
POST   /api/invoices
GET    /api/invoices/{id}/document
GET    /api/payments
POST   /api/payments
POST   /api/payments/{id}/refund
```

### Revenue and reporting

```text
GET    /api/revenue/summary
GET    /api/revenue/trend
GET    /api/revenue/top-products
GET    /api/reports/revenue
GET    /api/reports/inventory
```

### Audit logs

```text
GET    /api/audit-logs
GET    /api/audit-logs/{id}
GET    /api/audit-logs/export
```

---

## 19. Important Business Rules

1. SKU values must be unique.
2. Stock cannot become negative unless an authorised override is explicitly supported.
3. Sales order totals and GST must be calculated by the backend.
4. Product cost at the time of sale must be preserved for historical profit reporting.
5. Completed financial records should not be silently edited.
6. Refunds must reference an original payment.
7. Invoice numbers must be unique and sequential within the financial year.
8. A purchase order must be approved before normal stock receipt.
9. Stock changes must create an inventory transaction.
10. Critical create, update, delete, approval, payment, and export actions must create audit events.
11. Dates should be stored in UTC and displayed in IST.
12. Money should use fixed decimal database types, never floating-point types.

---

## 20. Recommended Implementation Order

### Phase 1 — Foundation

1. Authentication and authorisation
2. Application shell and navigation
3. Shared table, form, modal, and notification components
4. MySQL connection and migrations
5. Audit logging foundation

### Phase 2 — Core modules

1. Employee Management
2. Product Catalogue
3. Inventory balances and transactions
4. Supplier Management
5. Purchase Orders

### Phase 3 — Sales and finance

1. Customer Management
2. Sales Orders
3. GST Invoices
4. Payments and refunds
5. Revenue reporting

### Phase 4 — Operational improvements

1. Notifications
2. Advanced search and filters
3. Excel, CSV, and PDF exports
4. Dashboard optimisation
5. Automated tests
6. Monitoring and backup procedures

---

## 21. Prototype-to-Production Mapping

The current `template.html` is a visual and interaction prototype. It should be used as a reference when building Angular components.

| Prototype area | Angular target |
|---|---|
| Sidebar and top bar | Application layout components |
| Dashboard cards | Reusable KPI card component |
| Charts | Chart components backed by API data |
| Data tables | Shared server-side table component |
| Add/edit modals | Feature-specific reactive forms |
| Toast messages | Global notification service |
| Mock arrays and HTML rows | .NET API responses |
| Hard-coded INR values | Backend calculations and formatted values |
| Hash navigation | Angular Router |

The production Angular application should not copy the entire prototype into one component. It should preserve the design while separating each feature into maintainable components and services.

---

## 22. Complete Business Scenario

The following scenario shows how the modules work together:

1. Inventory identifies that iPhone 16 Pro Max stock is below its reorder level.
2. The system notifies the Warehouse Manager.
3. The Warehouse Manager creates a purchase order for Apple India.
4. An Administrator approves the purchase order.
5. The supplier ships the products.
6. The warehouse receives and verifies the shipment.
7. Inventory stock increases and the product returns to `IN_STOCK`.
8. A customer places an online sales order.
9. The backend verifies and reserves the required stock.
10. The customer pays through UPI.
11. A payment record and GST invoice are created.
12. Warehouse staff pick, pack, and ship the order.
13. The order is delivered and marked complete.
14. Revenue Management includes the payment in revenue and the product cost in cost of goods sold.
15. Every important action is available in Audit Logs.

This connected flow is the central behaviour of the application.

