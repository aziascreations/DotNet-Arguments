using NUnit.Framework;

namespace NibblePoker.Library.Arguments.Tests {
    public class TestRecursiveRegistration {
        // Used for other tests
        private Option _recursiveLongOption;

        // Used to test short options with value.
        private Option _recursiveShortOption;

        // Root verb
        private Verb _rootVerb;
        private Option _singleLongOption;
        private Option _singleShortOption;
        private Verb _subSubVerb;
        private Verb _subVerb;
        private Verb _subVerbAlt;

        [SetUp]
        public void Setup() {
            _rootVerb = new Verb(null);
            _subVerb = new Verb("sub");
            _subSubVerb = new Verb("sub-sub");
            _subVerbAlt = new Verb("alt");

            _recursiveShortOption = new Option('a', null);
            _singleShortOption = new Option('a', null);

            _recursiveLongOption = new Option(null, "alpha");
            _singleLongOption = new Option(null, "alpha");

            _subVerb.RegisterVerb(_subSubVerb);
            _rootVerb.RegisterVerb(_subVerb);
            _rootVerb.RegisterVerb(_subVerbAlt);
        }

        [Test]
        public void TestRecursiveEmptyShort() {
            // Testing the proper registration on an empty root verb for short options

            // Safety clear
            _rootVerb.Clear();
            Assert.DoesNotThrow(() => { _rootVerb.RegisterOptionRecursively(_recursiveShortOption); });

            Assert.Multiple(() => {
                Assert.That(_rootVerb.Options.Contains(_recursiveShortOption));
                Assert.That(_subVerb.Options.Contains(_recursiveShortOption));
                Assert.That(_subSubVerb.Options.Contains(_recursiveShortOption));
                Assert.That(_subVerbAlt.Options.Contains(_recursiveShortOption));
            });
        }

        [Test]
        public void TestRecursiveEmptyLong() {
            // Testing the proper registration on an empty root verb for long options

            // Safety clear
            _rootVerb.Clear();
            Assert.DoesNotThrow(() => { _rootVerb.RegisterOptionRecursively(_recursiveLongOption); });

            Assert.Multiple(() => {
                Assert.That(_rootVerb.Options.Contains(_recursiveLongOption));
                Assert.That(_subVerb.Options.Contains(_recursiveLongOption));
                Assert.That(_subSubVerb.Options.Contains(_recursiveLongOption));
                Assert.That(_subVerbAlt.Options.Contains(_recursiveLongOption));
            });
        }

        [Test]
        public void TestRecursiveDuplicateRoot() {
            // Testing with duplicate in root
            // We don't test sub-verbs as we'll assume they're in an unknown state.

            // Safety clear
            _rootVerb.Clear();
            Assert.DoesNotThrow(() => {
                _rootVerb.RegisterOption(_singleShortOption);
                _rootVerb.RegisterOption(_singleLongOption);
            });

            Assert.Throws<Exceptions.DuplicateOptionException>(delegate {
                _rootVerb.RegisterOptionRecursively(_recursiveShortOption);
            });

            Assert.Multiple(() => {
                Assert.That(_rootVerb.Options, Has.Count.EqualTo(2));
                Assert.That(_rootVerb.Options, Does.Contain(_singleShortOption));
                Assert.That(_rootVerb.Options, Does.Contain(_singleLongOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveShortOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveLongOption));
            });

            Assert.Throws<Exceptions.DuplicateOptionException>(delegate {
                _rootVerb.RegisterOptionRecursively(_recursiveLongOption);
            });

            Assert.Multiple(() => {
                Assert.That(_rootVerb.Options, Has.Count.EqualTo(2));
                Assert.That(_rootVerb.Options, Does.Contain(_singleShortOption));
                Assert.That(_rootVerb.Options, Does.Contain(_singleLongOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveShortOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveLongOption));
            });
        }

        [Test]
        public void TestRecursiveDuplicateIgnoredRoot() {
            // Testing with duplicate in root that are ignored
            // We will test sub-verbs as they should have the new option registered.

            // Safety clear
            _rootVerb.Clear();
            Assert.DoesNotThrow(() => {
                _rootVerb.RegisterOption(_singleShortOption);
                _rootVerb.RegisterOption(_singleLongOption);
            });

            Assert.DoesNotThrow(() => { _rootVerb.RegisterOptionRecursively(_recursiveShortOption, true); });

            Assert.Multiple(() => {
                Assert.That(_rootVerb.Options, Has.Count.EqualTo(2));
                Assert.That(_rootVerb.Options, Does.Contain(_singleShortOption));
                Assert.That(_rootVerb.Options, Does.Contain(_singleLongOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveShortOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveLongOption));

                Assert.That(_subVerb.Options, Has.Count.EqualTo(1));
                Assert.That(_subVerb.Options, Does.Contain(_recursiveShortOption));

                Assert.That(_subSubVerb.Options, Has.Count.EqualTo(1));
                Assert.That(_subSubVerb.Options, Does.Contain(_recursiveShortOption));

                Assert.That(_subVerbAlt.Options, Has.Count.EqualTo(1));
                Assert.That(_subVerbAlt.Options, Does.Contain(_recursiveShortOption));
            });

            Assert.DoesNotThrow(() => { _rootVerb.RegisterOptionRecursively(_recursiveLongOption, true); });

            Assert.Multiple(() => {
                Assert.That(_rootVerb.Options, Has.Count.EqualTo(2));
                Assert.That(_rootVerb.Options, Does.Contain(_singleShortOption));
                Assert.That(_rootVerb.Options, Does.Contain(_singleLongOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveShortOption));
                Assert.That(_rootVerb.Options, !Does.Contain(_recursiveLongOption));

                Assert.That(_subVerb.Options, Has.Count.EqualTo(2));
                Assert.That(_subVerb.Options, Does.Contain(_recursiveShortOption));
                Assert.That(_subVerb.Options, Does.Contain(_recursiveLongOption));

                Assert.That(_subSubVerb.Options, Has.Count.EqualTo(2));
                Assert.That(_subSubVerb.Options, Does.Contain(_recursiveShortOption));
                Assert.That(_subSubVerb.Options, Does.Contain(_recursiveLongOption));

                Assert.That(_subVerbAlt.Options, Has.Count.EqualTo(2));
                Assert.That(_subVerbAlt.Options, Does.Contain(_recursiveShortOption));
                Assert.That(_subVerbAlt.Options, Does.Contain(_recursiveLongOption));
            });
        }
    }
}
