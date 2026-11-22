import { useEffect, useState } from 'react';

import KpiCards from '../components/KpiCards';
import RegistrationFilters from '../components/RegistrationFilters';
import RegistrationTable from '../components/RegistrationTable';
import Pagination from '../components/Pagination';

import useDebounce from '../hooks/useDebounce';
import { getRegistrations, getSummary } from '../api/sample-data';

function RegistrationsPage() {
  const [registrations, setRegistrations] = useState([]);
  const [summary, setSummary] = useState(null);

  const [search, setSearch] = useState('');
  const [type, setType] = useState('');
  const [status, setStatus] = useState('');

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [retry, setRetry] = useState(0);

  const debouncedSearch = useDebounce(search, 400);

  useEffect(() => {
    async function loadDashboard() {
      try {
        setLoading(true);
        setError('');

        const [registrationData, summaryData] = await Promise.all([
          getRegistrations({
            participantName: debouncedSearch,
            type,
            status,
            page,
            pageSize,
          }),
          getSummary(),
        ]);

        setRegistrations(registrationData.items);
        setTotalCount(registrationData.totalCount);
        setTotalPages(registrationData.totalPages);
        setSummary(summaryData);
      } catch (err) {
        setError('Unable to load registrations. Please try again.');
      } finally {
        setLoading(false);
      }
    }

    loadDashboard();
  }, [debouncedSearch, type, status, page, pageSize, retry]);

  function clearFilters() {
    setSearch('');
    setType('');
    setStatus('');
    setPage(1);
  }

  function changeType(value) {
    setType(value);
    setPage(1);
  }

  function changeStatus(value) {
    setStatus(value);
    setPage(1);
  }

  function changePageSize(value) {
    setPageSize(value);
    setPage(1);
  }

  return (
    <div className="container-fluid py-4">
      <div className="mb-4">
        <h2 className="fw-bold">Car Show 2027</h2>
        <p className="text-muted">
          Registration Management Dashboard
        </p>
      </div>

      <KpiCards summary={summary} />

      <RegistrationFilters
        search={search}
        setSearch={(value) => {
          setSearch(value);
          setPage(1);
        }}
        type={type}
        setType={changeType}
        status={status}
        setStatus={changeStatus}
        onClear={clearFilters}
      />

      <div className="card shadow-sm">
        <div className="card-header bg-white py-3">
          <h5 className="mb-0">Registration Records</h5>
        </div>

        <div className="card-body">
          {error ? (
            <div className="alert alert-danger text-center">
              <p>{error}</p>
              <button
                className="btn btn-outline-danger"
                onClick={() => setRetry((value) => value + 1)}
              >
                Try Again
              </button>
            </div>
          ) : (
            <>
              <RegistrationTable
                registrations={registrations}
                loading={loading}
              />

              {!loading && (
                <Pagination
                  page={page}
                  totalPages={totalPages}
                  totalCount={totalCount}
                  pageSize={pageSize}
                  onPageChange={setPage}
                  onPageSizeChange={changePageSize}
                />
              )}
            </>
          )}
        </div>
      </div>
    </div>
  );
}

export default RegistrationsPage;