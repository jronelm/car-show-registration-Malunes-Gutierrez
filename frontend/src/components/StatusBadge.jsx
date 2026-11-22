function StatusBadge({ status }) {
  const statusColors = {
    Pending: 'bg-warning text-dark',
    Confirmed: 'bg-success',
    CheckedIn: 'bg-primary',
    Cancelled: 'bg-danger',
  };

  const displayStatus = status === 'CheckedIn' ? 'Checked In' : status;

  return (
    <span className={`badge ${statusColors[status] || 'bg-secondary'}`}>
      {displayStatus}
    </span>
  );
}

export default StatusBadge;