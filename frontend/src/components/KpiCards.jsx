import KpiCard from './KpiCard';

function KpiCards({ summary }) {
  const cards = [
    {
      title: 'Total Registrations',
      value: summary?.totalRegistrations ?? 0,
    },
    {
      title: 'Total Payments',
      value: `₱${(summary?.totalPayments ?? 0).toLocaleString('en-PH', {
        minimumFractionDigits: 2,
      })}`,
    },
    {
      title: 'Uncollected Payments',
      value: `₱${(summary?.uncollectedPayments ?? 0).toLocaleString('en-PH', {
        minimumFractionDigits: 2,
      })}`,
    },
    {
      title: 'Confirmed',
      value: summary?.confirmed ?? 0,
    },
    {
      title: 'Checked In',
      value: summary?.checkedIn ?? 0,
    },
  ];

  return (
    <div className="row g-3 mb-4">
      {cards.map((card) => (
        <div className="col-12 col-sm-6 col-lg" key={card.title}>
          <KpiCard title={card.title} value={card.value} />
        </div>
      ))}
    </div>
  );
}

export default KpiCards;