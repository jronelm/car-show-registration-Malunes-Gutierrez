function PaymentBadge({ paymentStatus }) {
  const paymentColors = {
    Unpaid: 'bg-danger',
    Partial: 'bg-warning text-dark',
    Paid: 'bg-success',
  };

  return (
    <span className={`badge ${paymentColors[paymentStatus] || 'bg-secondary'}`}>
      {paymentStatus || 'Unknown'}
    </span>
  );
}

export default PaymentBadge;