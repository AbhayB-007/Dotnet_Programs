import { Component, ErrorInfo, ReactNode } from 'react';

interface Props { children: ReactNode }
interface State { hasError: boolean }

// Class components are still the only way to implement an error boundary in
// React (no hook equivalent exists) — good to know even in a hooks-first app.
export class ErrorBoundary extends Component<Props, State> {
  state: State = { hasError: false };

  static getDerivedStateFromError(): State {
    return { hasError: true };
  }

  componentDidCatch(error: Error, info: ErrorInfo) {
    console.error('Uncaught UI error:', error, info);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="flex h-screen flex-col items-center justify-center gap-3 text-center">
          <h1 className="text-2xl font-semibold text-gray-800">Something went wrong</h1>
          <p className="text-gray-500">Please refresh the page. If the problem persists, contact support.</p>
          <button
            className="rounded-md bg-brand-500 px-4 py-2 text-white hover:bg-brand-600"
            onClick={() => window.location.reload()}
          >
            Reload
          </button>
        </div>
      );
    }
    return this.props.children;
  }
}
