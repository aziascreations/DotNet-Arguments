using System.Diagnostics;
using NUnit.Framework;

namespace NibblePoker.Library.Arguments.Tests {
	[TestFixture]
	public class TestVerbs {
		// Default verb
		private Verb _rootVerb = null!;

		// Valid sub verbs
		private Verb _validSubVerb1 = null!;
		private Verb _validSubVerb2 = null!;

		// Will throw an exception when registered into '_rootVerb'.
		// Verbs with spaces in their name aren't tested since they're allowed !
		private Verb _invalidSubVerb1 = null!;
		private Verb _invalidSubVerb2 = null!;

		// Used to test basic registration checks and getters.
		private Option _shortOption = null!;
		private Option _longOption = null!;
		private Option _mixedOption = null!;

		// TODO: Add defaults checks

		[SetUp]
		public void Setup() {
			_rootVerb = new Verb(null);

			_validSubVerb1 = new Verb("create");
			_validSubVerb2 = new Verb("delete");

			_invalidSubVerb1 = new Verb(null);
			_invalidSubVerb2 = new Verb("  ");

			_shortOption = new Option('a', null);
			_longOption = new Option(null, "bravo");
			_mixedOption = new Option('c', "charlie");
		}

		[Test]
		public void TestValidSubVerbs() {
			Assert.DoesNotThrow(() => {
				_rootVerb
					.RegisterVerb(_validSubVerb1)
					.RegisterVerb(_validSubVerb2);
			});

			Assert.Throws<Exceptions.DuplicateVerbException>(delegate { _rootVerb.RegisterVerb(_validSubVerb1); });
			Assert.Throws<Exceptions.DuplicateVerbException>(delegate { _rootVerb.RegisterVerb(_validSubVerb2); });

			Debug.Assert(_validSubVerb1.ParentVerb != null, "_validSubVerb1.ParentVerb != null");
			Debug.Assert(_validSubVerb2.ParentVerb != null, "_validSubVerb2.ParentVerb != null");
			Assert.Multiple(() => {
				Assert.That(_validSubVerb1.ParentVerb, Is.EqualTo(_rootVerb));
				Assert.That(_validSubVerb2.ParentVerb, Is.EqualTo(_rootVerb));
			});

			Assert.Multiple(() => {
				Assert.That(_rootVerb.GetSubVerbByName("create"), Is.EqualTo(_validSubVerb1));
				Assert.That(_rootVerb.GetSubVerbByName("delete"), Is.EqualTo(_validSubVerb2));
				Assert.That(_rootVerb.GetSubVerbByName("remove"), Is.Null);
			});
		}

		[Test]
		public void TestInvalidSubVerbs() {
			Assert.Throws<Exceptions.InvalidVerbNameException>(delegate { _rootVerb.RegisterVerb(_invalidSubVerb1); });
			Assert.Throws<Exceptions.InvalidVerbNameException>(delegate { _rootVerb.RegisterVerb(_invalidSubVerb2); });
		}

		[Test]
		public void TestOptions() {
			Assert.DoesNotThrow(() => {
				_rootVerb
					.RegisterOption(_shortOption)
					.RegisterOption(_longOption)
					.RegisterOption(_mixedOption);
			});

			Assert.Throws<Exceptions.DuplicateOptionException>(delegate { _rootVerb.RegisterOption(_shortOption); });
			Assert.Throws<Exceptions.DuplicateOptionException>(delegate { _rootVerb.RegisterOption(_longOption); });
			Assert.Throws<Exceptions.DuplicateOptionException>(delegate { _rootVerb.RegisterOption(_mixedOption); });

			Assert.Multiple(() => {
				Assert.That(_rootVerb.GetOptionByName("bravo"), Is.EqualTo(_longOption));
				Assert.That(_rootVerb.GetOptionByName("charlie"), Is.EqualTo(_mixedOption));
				Assert.That(_rootVerb.GetOptionByName("delta"), Is.Null);
			});

			Assert.Multiple(() => {
				Assert.That(_rootVerb.GetOptionByToken('a'), Is.EqualTo(_shortOption));
				Assert.That(_rootVerb.GetOptionByToken('c'), Is.EqualTo(_mixedOption));
				Assert.That(_rootVerb.GetOptionByToken('d'), Is.Null);
			});

			// Testing duplicates by token & name
			Assert.Throws<Exceptions.DuplicateOptionException>(delegate {
				_rootVerb.RegisterOption(new Option(_mixedOption.Token, null));
			});
			Assert.Throws<Exceptions.DuplicateOptionException>(delegate {
				_rootVerb.RegisterOption(new Option(null, _mixedOption.Name));
			});

			// TODO: Add defaults checks
		}
	}
}
