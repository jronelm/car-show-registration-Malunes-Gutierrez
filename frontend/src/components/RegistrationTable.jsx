import StatusBadge from './StatusBadge';
import PaymentBadge from './PaymentBadge';

function RegistrationTable({ registrations, loading }) {
  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status"></div>
        <p className="mt-2 text-muted">Loading registrations...</p>
      </div>
    );
  }

  if (!registrations || registrations.length === 0) {
    return (
      <div className="text-center py-5">
        <h5>No registrations found</h5>
        <p className="text-muted">Try changing your search or filters.</p>
      </div>
    );
  }

  return (
    <div className="table-responsive">
      <table className="table table-hover align-middle">
        <thead className="table-light">
          <tr>
            <th>Registration No.</th>
            <th>Date</th>
            <th>Participant</th>
            <th>Car</th>
            <th>Type</th>
            <th>Status</th>
            <th>Fee / Paid</th>
            <th>Payment</th>
          </tr>
        </thead>

        <tbody>
          {registrations.map((item) => (
            <tr key={item.id}>
              <td className="fw-semibold">{item.registrationNumber}</td>

              <td>
                {item.registrationDate
                  ? new Date(item.registrationDate).toLocaleDateString()
                  : 'N/A'}
              </td>

              <td>
                <div className="fw-semibold">{item.participantName}</div>
                <small className="text-muted">
                  {item.contactNumber}
                  <br />
                  {item.email}
                </small>
              </td>

              <td>
                <div>{item.carMakeModel}</div>
                <small className="text-muted">{item.plateNumber}</small>
              </td>

              <td>{item.registrationType}</td>

              <td>
                <StatusBadge status={item.status} />
              </td>

              <td>
                <div>
                  ₱{Number(item.registrationFee || 0).toLocaleString('en-PH', {
                    minimumFractionDigits: 2,
                  })}
                </div>
                <small className="text-muted">
                  Paid: ₱{Number(item.amountPaid || 0).toLocaleString('en-PH', {
                    minimumFractionDigits: 2,
                  })}
                </small>
              </td>

              <td>
                <PaymentBadge paymentStatus={item.paymentStatus} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default RegistrationTable;